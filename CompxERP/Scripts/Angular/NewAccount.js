var appA = angular.module("NewAcc", ['ngMaterial']).factory('Dialog', ['$mdDialog', '$timeout', '$mdToast',
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
}]);
 
appA.controller("ctrlNewAcc", function ($scope, $http)
{
     

    $scope.SaveAcc = function () {
             
        var DataArray = { Name: $scope.NewName, Address: $scope.NewAddress, GSTIN: $scope.NewGSTIN, AcGroup: $scope.NewMobile };
        var $promise = $http.post('../Voucher/AddAccount', DataArray);
        debugger;
        $promise.then(function (response1) {
            $mdDialog.hide(response1); 
        }); 
    }
}
); 
 
appA.controller("ctrlOpenAcc", ['$scope', '$mdDialog', '$http', function ($scope, $mdDialog, $http) {
   
    $scope.OpenAcc = function (ev, cntrname) { 
     debugger;
     if ($scope.tpPartyID == angular.isUndefinedOrNull || $scope.tpPartyID == "" || $scope.tpPartyID == "0") {

         $mdDialog.show({
             controller: 'ctrlNewAcc',
             templateUrl: '../Voucher/NewAccount',
             parent: angular.element(document.body),
             targetEvent: ev,
             clickOutsideToClose: false,
         })
      }
     else {
         $mdDialog.hide();
     }
    }
    $scope.onBlur = function () {
        alert('aa1');
        console.log();
        console.log($scope.model);
        debugger;
        alert($scope.txtID); 
        alert($scope.tpPartyID);
        alert($scope.txtName);
        if ($scope.tpPartyID == angular.isUndefinedOrNull || $scope.tpPartyID == "" || $scope.tpPartyID == "0") {

            $mdDialog.show({
                controller: 'ctrlNewAcc',
                templateUrl: '../Voucher/NewAccount',
                parent: angular.element(document.body),
                targetEvent: ev,
                clickOutsideToClose: false,
            })
        }
        else {
            $mdDialog.hide();
        }

    };
}
]);
 
appA.controller("CtrlFilter", function ($scope, $http)
{
    debugger;
var CityArr = []; 
   
$scope.ff = [];
$scope.ff = $http.get("../Report/GetCityArr");
  
$http({
    method: "get",
    url: "../Report/GetCityArr"
}).then(function (response) {
    $scope.ff = response.data;


    angular.forEach($scope.ff.lstCity, function (value, index) {
        var obj = new Object();
        obj.CityNm = value.PartyName,
        obj.CityID = value.PartyID
        CityArr.push(obj);
    });

    $scope.items = CityArr;
  
}, function () {
    alert("Error Occur");
});
 

});