angular.module('CompXApp', ['ngMaterial'])//, 'ngMessages', 'material.svgAssetsCache'
alert('aaa')
    .factory('Dialog', ['$mdDialog', '$timeout', '$mdToast',
function ($mdDialog, $timeout, $mdToast) {
    return {
        autohide: function (message, timeout_time) {
            $mdDialog.show({
                clickOutsideToClose: false, escapeToClose: false,
                template: "<md-dialog style='max-width:100%;'><md-toolbar style='padding:20px 40px;background-color:#DFF0D8;color:#4f8a10;'><div class='md-toolbar-tools'><h2 style='font-weight:700;font-size:19px;'> " + message + "  </h2></div></md-toolbar></md-dialog>",
                fullscreen: false, locals: { timeout_time: timeout_time },
                controller: function DialogController($mdDialog, $timeout, timeout_time) {
                    if (timeout_time == angular.isUndefinedOrNull) timeout_time = 3000;
                    $timeout(callAtTimeout1, timeout_time);
                    function callAtTimeout1() { $mdDialog.hide(); }
                }
            });
        },
        show: function (title, message, event) {
            $mdDialog.show(
                $mdDialog.alert()
                .title(title)
                .content(message)
                .ariaLabel('')
                .ok('Got it!')
                .targetEvent(event)
            );
        },
        Error: function (title, message, event) {
            $mdDialog.show(
                $mdDialog.alert()
                .title(title)
                .content(message)
                .ariaLabel('')
                .ok('Got it!')
                .targetEvent(event)
            );
        },
        SessionExpried: function (message, event) {
            $mdDialog.show(
                $mdDialog.alert()
                .title('Session Expired')
                .content(message)
                .ariaLabel('')
                .ok('Got it!')
                .targetEvent(event)
            );
        },
        autoToast: function (message, timeout_time) {
            if (timeout_time == angular.isUndefinedOrNull) timeout_time = 3000;
            $mdToast.show({
                template: '<md-toast class="md-toast success">' + message + '</md-toast>',
                hideDelay: timeout_time,
                position: 'top right'
            });
        }
    }
}])
.controller('cntrlSaudaExpo', ['$scope', '$mdDialog', '$http', function ($scope, $mdDialog, $http) {//Use In SaudaExpo
    $scope._chgv = "0";
    var DataArray = { x: 905 };
    $scope.sKotItem = [];
    var $promise = $http.post('GetTermsType', DataArray);
    $promise.then(function (response1) {

        $scope.TermNames = response1.data;
    });
    var $promise = $http.post('GetTermsCondition');
    $promise.then(function (response1) {
        $scope.TermnCondition = response1.data;
    });
    var $promise = $http.post('GetPort');
    $promise.then(function (response1) {
        $scope.PortInfo = response1.data;
        $scope.sePortID = $scope.PortInfo[0].code;
        $scope.sePortIDTo = $scope.PortInfo[0].code;
    });
    var $promise = $http.post('GetCountry');
    $promise.then(function (response1) {
        $scope.CountryInfo = response1.data;
        $scope.countryID = $scope.CountryInfo[0].code;
    });
    $scope.openTermPop = function (val) {
        $scope._chgv = val;
    }
    $scope.addTermnCondition = function () {

        var inData1 = JSON.stringify({ 'Data': $scope.sKotItem });
        //var inData = { 'Data': JSON.stringify($scope.sKotItem) };
        $scope.TermnCondition1 = inData1;
    }
    $scope.add_item = function (tnc, index) {//(subcatCode, index, name, tableid, kotno) {//sourabh
        if ($scope.sKotItem.length > 0) {
            $scope.sKotItemBool = true;
            var vRemoveThenAdd = -1;
            $scope.sKotItem.find(function (item, itmeindex) {
                if (item.code == tnc.code) {
                    vRemoveThenAdd = itmeindex;
                }
            });
            if (vRemoveThenAdd != -1) {
                $scope.sKotItem.splice(vRemoveThenAdd, 1);
            }
        }
        if (tnc.IsSelected) {
            $scope.sKotItem.push({
                IsPrcnt: tnc.IsPrcnt,
                IsSelected: tnc.IsSelected,
                TaxPer: tnc.TaxPer,
                code: tnc.code,
                text: tnc.text,
                tnType: tnc.tnType,
            });
        }
        $scope.addTermnCondition();
    };
    //$scope.getInveSettinfo = function () {
    //    $http({
    //        method: 'POST',
    //        url: 'CompanyMaster.aspx/getInveSettinfo',
    //        data: {}
    //    }).success(function (result) {
    //        $scope.InveSettList = result.d;
    //    });
    //};
    //$scope.getInveSettinfo();
    //$scope.setInveSettinfo = function (a, b) {
    //    $http({ method: 'POST', url: 'CompanyMaster.aspx/setInveSettinfo', data: { a: a, b: b } }).success(function (result) {
    //        $mdToast.show($mdToast.simple().textContent(result.d).parent(document.querySelectorAll('#toaster')).hideDelay(1000));
    //    });
    //}
    $scope.openCreatePort = function (ev, cntrname) {
        $scope.cntrname = cntrname;
        if ($scope.countryID != angular.isUndefinedOrNull && $scope.countryID != "") {
            $mdDialog.show({
                controller: 'cntrlPortInfo',
                templateUrl: '../Master/CreatePort',
                parent: angular.element(document.body),
                targetEvent: ev,
                clickOutsideToClose: false,
            })
          .then(function (result) {
              if (result != angular.isUndefinedOrNull) {
                  var $promise = $http.post('GetPort');
                  $promise.then(function (response1) {
                      $scope.PortInfo = response1.data;
                      if ($scope.cntrname == 1) $scope.sePortID = result.data;
                      else $scope.sePortIDTo = result.data;
                  });
              }
          }, function () { });
        } else Dialog.autohide('Please Select Country First...');
    };
    $scope.openCreateCountry = function (ev) {
        $mdDialog.show({
            controller: 'cntrlCountryInfo',
            templateUrl: '../Master/CreateCountry',
            parent: angular.element(document.body),
            targetEvent: ev,
            clickOutsideToClose: false,
        })
      .then(function (result) {
          if (result != angular.isUndefinedOrNull) {
              var $promise = $http.post('GetCountry');
              $promise.then(function (response1) {
                  $scope.CountryInfo = response1.data;
                  $scope.countryID = result.data;
              });
          }
      }, function () { });
    };
}])
.controller('cntrlPortInfo', ['$scope', '$mdDialog', '$http', 'Dialog',
    function ($scope, $mdDialog, $http, Dialog) {//Use In SaudaExpo,CreatePort
        var $promise = $http.post('GetCountry');
        $promise.then(function (response1) {
            $scope.CountryInfo = response1.data;
            $scope.studcity = $scope.CountryInfo[0].code;
        });
        $scope.hide = function () { $mdDialog.hide(); }
        $scope.SetPort = function () {
            if ($scope.studcity == angular.isUndefinedOrNull || $scope.studcity == "") {
                Dialog.autoToast('Please Select Country...');
            }
            else if ($scope.studname == angular.isUndefinedOrNull || $scope.studname == "") {
                Dialog.autoToast('Please Feed Port Name...');
            }
            else {
                var DataArray = { a: $scope.studname, b: $scope.studcity, c: $scope.studAlia };
                var $promise = $http.post('../Master/SetPort', DataArray);
                $promise.then(function (response1) {
                    $mdDialog.hide(response1);
                });
            }
        }
    }])
    .controller('cntrlCountryInfo', ['$scope', '$mdDialog', '$http', 'Dialog',
    function ($scope, $mdDialog, $http, Dialog) {//Use In SaudaExpo,CreateCountry
        $scope.hide = function () { $mdDialog.hide(); }
        $scope.SetCountry = function () {
            if ($scope.cityname == angular.isUndefinedOrNull || $scope.cityname == "") {
                Dialog.autoToast('Please Feed Country...');
            }
            else {
                var DataArray = { a: $scope.cityname, b: $scope.cityalia };
                var $promise = $http.post('../Master/SetCountry', DataArray);
                $promise.then(function (response1) {
                    $mdDialog.hide(response1);
                });
            }
        }
    }]);