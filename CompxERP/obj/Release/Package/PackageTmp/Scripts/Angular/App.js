var app = angular.module('CompXApp', ['ngMaterial']);//, 'ngMessages', 'material.svgAssetsCache'

app.factory('Dialog', ['$mdDialog', '$timeout', '$mdToast',
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
.controller('cntrlPortInfo', ['$scope', '$mdDialog', '$http', 'Dialog', function ($scope, $mdDialog, $http, Dialog) {//Use In SaudaExpo,CreatePort
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
.controller('cntrlCountryInfo', ['$scope', '$mdDialog', '$http', 'Dialog', function ($scope, $mdDialog, $http, Dialog) {//Use In SaudaExpo,CreateCountry
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
}])
.controller('cntrlAccountSetup', ['$scope', '$mdDialog', '$http', 'Dialog', function ($scope, $mdDialog, $http, Dialog) {
    $scope.openAccountCategory = function (ev) {
        $mdDialog.show({
            controller: 'cntrlAddAccount_Category',
            templateUrl: '../Master/AddAccount_Category',
            parent: angular.element(document.body), targetEvent: ev, clickOutsideToClose: false,
        })
      .then(function (result) {
          if (result != angular.isUndefinedOrNull) {
              var d = result.data;
              var $promise = $http.post('../AcctMst/GetAcctCateList');
              $promise.then(function (data) {
                  $("#acctcate").empty();
                  $.each(data.data, function () { $("#acctcate").append($("<option />").val(this.Value).text(this.Text)); });
                  $("#acctcate").val(d);
              });
          }
      }, function () { });

    }
}])
.controller('cntrlAddAccount_Category', ['$scope', '$mdDialog', '$http', 'Dialog', function ($scope, $mdDialog, $http, Dialog) {
    $scope.hide = function () { $mdDialog.hide(); }
    $scope.SetAccountCategory = function () {
        if ($scope.txtCatName == angular.isUndefinedOrNull || $scope.cityname == "") {
            Dialog.autoToast('Please Feed Category Name...');
        }
        else {
            $scope.txtAlias = $("#txtAlias").val();
            var DataArray = { Name: $scope.txtCatName, Alia: $scope.txtAlias };
            var $promise = $http.post('../Master/InsAccountCategory', DataArray);
            $promise.then(function (response1) {
                $mdDialog.hide(response1);
            });
        }
    }
}])


 .controller('cntrlSetSubCat', ['$scope', '$mdDialog', '$http', 'Dialog', function ($scope, $mdDialog, $http, Dialog) {
     $scope.category = $('#CatID').val();
     $scope.hide = function () { $mdDialog.hide(); }
     $scope.SetSubCatItem = function () {

         var Sort = 0; var Alia = '';
         if ($('#itgpsort').val() != "") Sort = $scope.itgpsort;
         if ($('#itgpalia').val() != "") Alia = $scope.itgpalia;

         debugger;
         if ($('#itgpname').val() == angular.isUndefinedOrNull || $('#itgpname').val() == "") {
             Dialog.autoToast('Please Enter Sub Category Name ...'); $('#itgpname').focus();
         }
         else if ($('#itgpbcqt').val() == angular.isUndefinedOrNull || $('#itgpbcqt').val() == "") {
             Dialog.autoToast('Please Enter Rate ...'); $('#itgpbcqt').focus();
         }
         else if ($('#category').val() == angular.isUndefinedOrNull || $('#category').val() == "") {
             Dialog.autoToast('Please Select Category ...'); $('#category').focus();
         }
         else if ($('#itgprefn').val() == angular.isUndefinedOrNull || $('#itgprefn').val() == "") {
             Dialog.autoToast('Please Enter HSN ...'); $('#itgprefn').focus();
         }
         else if ($('#itgpbasi').val() == angular.isUndefinedOrNull || $('#itgpbasi').val() == "") {
             Dialog.autoToast('Please Select Unit ...'); $('#itgpbasi').focus();
         }
         else {
             var data = new FormData()
             data.append("itgpalia", Alia);
             data.append("itgpsort", Sort);
             data.append("itgpbcqt", $('#itgpbcqt').val()); //$scope.itgpbcqt
             data.append("itgptype", $('#itgptype').val()); //$scope.itgptype
             data.append("itgpunde", $('#category').val()); //$scope.category
             data.append("itgpname", $('#itgpname').val()); //$scope.itgpname
             data.append("itgprefn", $('#itgprefn').val()); //$scope.itgprefn
             data.append("itgpcart", $('#itgpcart').val()); //$scope.itgpcart
             data.append("itgpbasi", $('#itgpbasi').val()); //$scope.itgpbasi
             data.append("itgpcode", $('#itgpcode').val());

             var $promise = $http({
                 url: '../ItemSetup/CreateSubType',
                 method: 'POST',
                 data: data,
                 headers: { 'Content-Type': undefined },
                 transformRequest: angular.identity
             });
             $promise.then(function (response1) {
                 debugger;

                 var ScArr = [];
                 ScArr.push({
                     SubCatID: response1.data,
                     CatID: $('#category').val() //$scope.category
                 })
                 // $scope.itgpcode = ""; $scope.itgpsort = 0; $scope.itgpalia = 0; 
                 $('#itgpcode').val('0'); $('#itgpsort').val(''); $('#itgpalia').val(''); $('#itgpname').val(''); $('#itgpbcqt').val('1'); $('#itgptype').val(0);
                 $('#itgprefn').val(''); $('#itgpcart').val(''); $('#itgpbasi').val(0);
                 $scope.ShowSubCategory();
                 $mdDialog.hide(ScArr);
             });
         }
     }

     $scope.OpenAddCat = function (ev) {
         debugger;

         $mdDialog.show({
             skipHide: true,
             controller: 'cntrlSetCat',
             templateUrl: '../ItemSetup/CreateType',
             parent: angular.element(document.body), targetEvent: ev, clickOutsideToClose: false, fullscreen: $scope.customFullscreen
         }).then(function (result) {
             if (result != angular.isUndefinedOrNull) {
                 debugger;
                 var $promise = $http.post('../DDL/GetCategory');
                 $promise.then(function (data) {
                     $.each(data.data, function () { $("#category").append($("<option />").val(this.Value).text(this.Text)); });
                     $('#category').val(result);
                     $('#itgpname').focus();
                     //focus($('#itgpalia').focus());
                 });
             }
         }, function () { });;
     }

     $scope.ShowSubCategory = function () {
         debugger;
         var $promise = $http.post('../DDL/GetAllSubCategory?CatID=' + $('#category').val());
         $promise.then(function (response1) {
             debugger;
             $scope.getSubCategory = response1.data; 
         });

     }

     $scope.EditSubCate = function (itm) {
         debugger;
         $('#itgpname').val(itm.itgpname);
         $('#itgpalia').val(itm.itgpalia);
         $('#itgpbcqt').val(itm.itgpbcqt);
         $('#itgptype').val(itm.itgptype);
         $('#itgpcode').val(itm.itgpcode);
         $('#itgpsort').val(itm.itgpsort);
         $('#itgpbasi').val(itm.itgpbasi);
         $('#itgprefn').val(itm.itgprefn);
         $('#itgpcart').val(itm.itgpcart);
     }

     $scope.DelSubCate = function (itm) {
         debugger;
         var DataArray = { itgpcode: itm.itgpcode };
         var $promise = $http.post('../ItemSetup/DelSubCate', DataArray);
         $promise.then(function (response1) {
             debugger;
             Dialog.autoToast(response1.data);
             $scope.ShowSubCategory();
         });

     }
 }])

 .controller('cntrlSetCat', ['$scope', '$mdDialog', '$http', 'Dialog', function ($scope, $mdDialog, $http, Dialog) {
     $scope.CatID = $('#category').val();
     $scope.hide = function () { $mdDialog.hide(); }
     $scope.SetCatItem = function () {
         debugger;
         if ($('#M_Category').val() == "") {
             Dialog.autoToast('Please Enter Category Name ...'); $('#M_Category').focus();
         }
         else {
             var Sort = 0; var Alia = '';
             if ($('#M_itgpsort').val() != "") Sort = $('#M_itgpsort').val();
             if ($('#M_itgpalia').val() != "") Alia = $('#M_itgpalia').val();

             var data = new FormData()
             data.append("itgpcode", $('#M_itgpcode').val());
             data.append("itgpname", $('#M_Category').val()); //data.append("category", $('#M_Category').val());
             data.append("itgpsort", Sort);
             data.append("itgpalia", Alia);
             data.append("isEdit", 1);
             var $promise = $http({
                 url: '../ItemSetup/CreateType',
                 method: 'POST',
                 data: data,
                 headers: { 'Content-Type': undefined },
                 transformRequest: angular.identity
             });
             $promise.then(function (response1) {
                 debugger;
                 //$scope.itgpcode = ""; $scope.category = ""; 
                 //$scope.itgpcode = 0; $scope.itgpsort = 0; $scope.itgpalia = 0; 
                 $('#M_Category').val(''); $('#M_itgpsort').val(''); $('#M_itgpalia').val(''); $('#M_itgpcode').val(0);

                 $scope.ShowCategory();
                 $mdDialog.hide(response1.data);
             });
         }
     }

     $scope.ShowCategory = function () {
         debugger;

         var $promise = $http.post('../DDL/GetAllCategory');
         $promise.then(function (response1) {
             debugger;
             $scope.getCategory = response1.data;
         });
         
     }
     $scope.edit = function (itm) {
         debugger;
         $('#M_Category').val(itm.itgpname);
         $('#M_itgpcode').val(itm.itgpcode);
         $('#M_itgpalia').val(itm.itgpalia);
         $('#M_itgpsort').val(itm.itgpsort);
     }
     $scope.DelCate = function (itm) {
         debugger;

         var DataArray = { itgpcode: itm.itgpcode };
         var $promise = $http.post('../ItemSetup/DelCate', DataArray);
         $promise.then(function (response1) {
             debugger;
             Dialog.autoToast(response1.data);
             $scope.ShowCategory();
         });

     }
 }])

 .controller('cntrlOpnItem', ['$scope', '$mdDialog', '$http', 'Dialog', function ($scope, $mdDialog, $http, Dialog) {

     $scope.hide = function () { $mdDialog.hide(); }
     $scope.OpenItem = function (ev) {
         debugger;
         $('#itemname').focus();
         $mdDialog.show({
             skipHide: true,
             controller: 'cntrlSetItem',
             templateUrl: '../../ItemSetup/Itemain',
             parent: angular.element(document.body), targetEvent: ev, clickOutsideToClose: false, fullscreen: $scope.customFullscreen
         });
         // $("#itemname").val($scope.xy);
         //$("#itemname").focus();

     }

     $scope.ChkmstChNo = function (ev) {
         debugger;
         var DataArray = { sVoucher: $('#mstchnH').val() + $('#mstchnV').val(), MstType: $('#msttype').val(), MstCode: $('#mstcode').val() };
         var $promise = $http.post('../../Voucher/CheckVoucher', DataArray);
         $promise.then(function (response1) {
             debugger;
             if (response1.data != 0) {
                 //$('#mstchnV').val('');
                 Dialog.autoToast('Ref. No Already Exists ...');
             }
         });


     }

     $scope.OpenCity = function (ev) {
         debugger;
         $('#CityName').focus();
         $mdDialog.show({
             skipHide: true,
             controller: 'cntrlCity',
             templateUrl: '../../Master/CityMaster',
             parent: angular.element(document.body), targetEvent: ev, clickOutsideToClose: false, fullscreen: $scope.customFullscreen
         });

     }
 }])

 .controller('cntrlSetItem', ['$scope', '$mdDialog', '$http', 'Dialog', function ($scope, $mdDialog, $http, Dialog) {
     $scope.itemname = $("#ItemNm").val();

     $scope.hide = function () { $mdDialog.hide(); }
     $scope.SetItem = function () {
         debugger;
         var Sort = 0; var Alia = ''; var Refn = 0; var Vat = 0; var Text = ''; var itemcode = 0; var itempart = 0;
         if ($('#itemcode').val() != angular.isUndefinedOrNull && $('#itemcode').val() != "") itemcode = $('#itemcode').val();
         if ($('#itemsort').val() != angular.isUndefinedOrNull && $('#itemsort').val() != "") Sort = $('#itemsort').val();
         if ($('#itemalia').val() != angular.isUndefinedOrNull && $('#itemalia').val() != "") Alia = $('#itemalia').val();
         if ($('#itemrefn').val() != angular.isUndefinedOrNull && $('#itemrefn').val() != "") Refn = $('#itemrefn').val();
         if ($('#itemvatr').val() != angular.isUndefinedOrNull && $('#itemvatr').val() != "") Vat = $('#itemvatr').val();
         if ($('#itemtext').val() != angular.isUndefinedOrNull && $('#itemtext').val() != "") Text = $('#itemtext').val();
         if ($('#itempart').val() != angular.isUndefinedOrNull && $('#itempart').val() != "") itempart = $('#itempart').val();

         if ($('#CatID').val() == angular.isUndefinedOrNull || $('#CatID').val() == "") {
             Dialog.autoToast('Please Select Category ...'); $('#CatID').focus();
         }
         else if ($('#SubCatID').val() == angular.isUndefinedOrNull || $('#SubCatID').val() == "" || $('#SubCatID').val() == "0") {
             Dialog.autoToast('Please Select Sub Category ...'); $('#SubCatID').focus();
         }
         else if ($('#itemname').val() == angular.isUndefinedOrNull || $('#itemname').val() == "") {
             Dialog.autoToast('Please Enter Item Name ...'); $('#itemname').focus();
         }
         else if ($('#itemhsnc').val() == angular.isUndefinedOrNull || $('#itemhsnc').val() == "") {
             Dialog.autoToast('Please Enter HSN ...'); $('#itemhsnc').focus();
         }
         else {
             $scope.txtAlias = $("#itemalia").val();
             //var DataArray = { Name: $scope.txtCatName, Alia: $scope.txtAlias };
             var data = new FormData()
             data.append("itemname", $('#itemname').val()); //$scope.itemname);
             data.append("itemalia", Alia);
             data.append("CatID", $('#CatID').val()); // $scope.CatID);
             data.append("SubCatID", $('#SubCatID').val()); // $scope.SubCatID
             data.append("itemnumb", $('#itemnumb').val()); // $scope.itemnumb); 
             data.append("itemsort", Sort);
             data.append("itemtext", Text);
             data.append("itemcess", $('#itemcess').val()); // $scope.itemcess);
             data.append("itemgstr", $('#itemgstr').val()); // $scope.itemgstr);
             data.append("itemrefn", Refn);
             data.append("itemhsnc", $('#itemhsnc').val()); // $scope.itemhsnc);
             data.append("itemvatr", Vat);
             data.append("itempart", itempart);
             data.append("itemcode", itemcode);
             console.log(typeof data);

             var $promise = $http({
                 url: '../../ItemSetup/Itemain',
                 method: 'POST',
                 data: data,
                 headers: { 'Content-Type': undefined },
                 transformRequest: angular.identity
             });
             $promise.then(function (response1) {
                 debugger;
                 //$scope.itemname = ""; $scope.itemalia = ""; $scope.itemnumb = 0; $scope.itempart = 0;
                 //$scope.itemsort = ""; $scope.itemtext = ""; $scope.itemcess = ""; $scope.itemgstr = "";
                 //$scope.itemrefn = ""; $scope.itemhsnc = ""; $scope.itemvatr = ""; //$scope.SubCatID = 0;$scope.CatID = 0; 

                 $('#itemname').val(""); $('#itemalia').val(""); $('#itemsort').val(""); $('#itemtext').val(""); $('#itemcess').val(""); $('#itemgstr').val("");
                 $('#itemrefn').val(""); $('#itemhsnc').val(""); $('#itemvatr').val("");
                 $('#itemnumb').val("0"); $('#itempart').val("0");
                 $('#itemcode').val("0");
                 $('#ItemID').val(response1.data);
                 $('#itemname').focus();
                 $scope.ShowItemList();
                 $mdDialog.hide(response1);
             });
         }
     }
     $scope.OpenAddSubCat = function (ev) {
         debugger;

         $mdDialog.show({
             skipHide: true,
             controller: 'cntrlSetSubCat',
             templateUrl: '../ItemSetup/CreateSubType',
             parent: angular.element(document.body), targetEvent: ev, clickOutsideToClose: false, fullscreen: $scope.customFullscreen
         })
         .then(function (result) {
             if (result != angular.isUndefinedOrNull) {
                 debugger;
                 var $promise = $http.post('../DDL/GetSubCategory?CatID=' + result[0].CatID);
                 $promise.then(function (data) {
                     $.each(data.data, function () { $("#SubCatID").append($("<option />").val(this.Value).text(this.Text)); });
                     $('#SubCatID').val(result[0].SubCatID);
                     $('#CatID').val(result[0].CatID);
                 });
             }
         }, function () { });
     }

     $scope.ShowItemList = function () {
         debugger;
         var $promise = $http.post('../ItemSetup/GetItemList?SubCatID=' + $('#SubCatID').val());
         $promise.then(function (response1) {
             debugger;
             $scope.GetItemList = response1.data;
         });

     }

     $scope.OpenAddBrand = function (ev) {
         debugger;

         $mdDialog.show({
             skipHide: true,
             controller: 'cntrlStudDet',
             templateUrl: '../ItemSetup/StudDet',
             parent: angular.element(document.body), targetEvent: ev, clickOutsideToClose: false, fullscreen: $scope.customFullscreen
         })
         .then(function (result) {
             if (result != angular.isUndefinedOrNull) {
                 debugger;
                 var $promise = $http.post('../DDL/GetStudDet?studtype=61');
                 $promise.then(function (data) {
                     $.each(data.data, function () { $("#itemnumb").append($("<option />").val(this.Value).text(this.Text)); });
                     $('#itemnumb').val(result.data);
                 });
             }
         }, function () { });
     }
     $scope.FillItemValue = function (itm) {

         debugger;
         $('#itemname').val(itm.itemname);
         $('#itemalia').val(itm.itemalia);
         $('#itemsort').val(itm.itemsort);
         $('#itemtext').val(itm.itemtext);
         $('#itemvatr').val(itm.itemvatr);

         $('#itemhsnc').val(itm.itemhsnc);
         $('#itemrefn').val(itm.itemrefn);
         $('#itemgstr').val(itm.itemgstr);
         $('#itemcess').val(itm.itemcess);
         $('#itemnumb').val(itm.itemnumb);
         $('#itemcode').val(itm.itemcode);
         $('#itempart').val(itm.itempart);
         //var Sort = 0; var Alia = ''; var Refn = 0;var Vat = 0;var Text = '';
         //       if ($scope.itemsort != angular.isUndefinedOrNull && $scope.itemsort != "") Sort = $scope.itemsort;
         //       if ($scope.itemalia != angular.isUndefinedOrNull && $scope.itemalia != "") Alia = $scope.itemalia;
         //       if ($scope.itemrefn != angular.isUndefinedOrNull && $scope.itemrefn != "") Refn = $scope.itemrefn;
         //       if ($scope.itemvatr != angular.isUndefinedOrNull && $scope.itemvatr != "") Vat = $scope.itemvatr;
         //       if ($scope.itemtext != angular.isUndefinedOrNull && $scope.itemtext != "") Text = $scope.itemtext;

     }

     $scope.DelItem = function (itm) {
         debugger;
         var DataArray = { itemcode: itm.itemcode };
         var $promise = $http.post('../ItemSetup/DelItem', DataArray);
         $promise.then(function (response1) {
             debugger;
             Dialog.autoToast(response1.data);
             $scope.ShowItemList();
         });

     }

 }])


.controller('cntrlVoucher', ['$scope', '$mdDialog', '$http', 'Dialog', function ($scope, $mdDialog, $http, Dialog) {

    $scope.CheckVoucher = function (ev) {
        debugger;
        var DataArray = { sVoucher: $('#mstchno').val(), MstType: $('#msttype').val(), MstCode: $('#mstcode').val() };
        var $promise = $http.post('CheckVoucher', DataArray);
        $promise.then(function (response1) {
            debugger;
            $('#MstChNo_Exists').val(response1.data);
            if (response1.data != 0) {
                Dialog.autoToast('Voucher No Already Exists ...');
            }
        });


    }
}])


.controller('cntrlStudDet', ['$scope', '$mdDialog', '$http', 'Dialog', function ($scope, $mdDialog, $http, Dialog) {
    $scope.hide = function () { $mdDialog.hide(); }

    $scope.SetStudDet = function (ev) {
        debugger;

        if ($scope.studName == angular.isUndefinedOrNull || $scope.studName == "") {
            Dialog.autoToast('Please Enter Name ...'); $('#studName').focus();
        }
        else {
            var DataArray = { studName: $('#studName').val() };
            var $promise = $http.post('../ItemSetup/SetStudDet', DataArray);
        }
        $promise.then(function (response1) {
            debugger;
            $('#studName').focus();
            $mdDialog.hide(response1);
        });

    }

    $scope.ShowStudDet = function () {
        debugger;
        var $promise = $http.post('../DDL/GetStudDet?studtype=61');
        $promise.then(function (response1) {
            debugger;
            $scope.getStudDet = response1.data;
        });

    }
}])

.controller('cntrlCity', ['$scope', '$mdDialog', '$http', 'Dialog', function ($scope, $mdDialog, $http, Dialog) {
    // $scope.hide = function () { $mdDialog.hide(); }

    $scope.InsCity = function (ev) {
        debugger;

        var CityCode = 0;
        if ($('#CityCode').val() != angular.isUndefinedOrNull && $('#CityCode').val() != "") CityCode = $('#CityCode').val();

        if ($('#CityName').val() == angular.isUndefinedOrNull || $('#CityName').val() == "") {
            Dialog.autoToast('Please Enter City Name ...'); $('#CityName').focus();
        }
        else if ($('#State').val() == angular.isUndefinedOrNull || $('#State').val() == "") {
            Dialog.autoToast('Please Enter State Name ...'); $('#State').focus();
        }
        else {
            var DataArray = { StateID: $('#State').val(), CityName: $('#CityName').val(), iCityCode: CityCode };
            var $promise = $http.post('../Master/InsCity', DataArray);
        }
        $promise.then(function (response1) {
            debugger;
            $('#CityName').val(''); $('#CityCode').val('0'); $('#CityName').focus();
            // $mdDialog.hide(response1);
            $scope.ShowCity();
        });

    }

    $scope.ShowCity = function () {
        debugger;
        var $promise = $http.post('../Master/GetCityList?iState=' + $("#State").val());
        $promise.then(function (response1) {
            debugger;
            $scope.getCity = response1.data;
            
        });

    }

    $scope.CityEdit = function (itm) {
        debugger;

        $('#CityName').val(itm.cityname);
        $('#CityCode').val(itm.citycode);
    }
}])



