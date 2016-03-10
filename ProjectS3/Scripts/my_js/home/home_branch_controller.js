﻿app.controller('home_branch_controller', ['$scope', '$scope', '$rootScope', '$http', 'my_function_services', 'cart_service',
    function ($scope, $scope, $rootScope, $http, my_function_services, cart_service) {
        // new variables
        $scope.BannerImages = [];
        $scope.myCount = 0;
        $scope.products = [];
        $scope.catelories = [];

        $scope.init = function (sanpham, myimagebanner) {
            cart_service.init();
            $scope.initBannerImage(myimagebanner);
            for (var i = 0; i < sanpham.length; i++) {
                // pasre and add to list image
                $scope.listproductimage = my_function_services.pasreimg(sanpham[i].linkImage);

                var myObject = {};
                myObject.ID = sanpham[i].ID
                myObject.Name = sanpham[i].Name;
                myObject.Price = sanpham[i].Price;
                myObject.BranchID = sanpham[i].BranchID;
                myObject.BranchName = sanpham[i].BranchName;
                myObject.TypeID = sanpham[i].TypeID;
                myObject.TypeName = sanpham[i].TypeName;
                myObject.isShow = true;

                myObject.currentImage = $scope.listproductimage[0];

                $scope.products.push(myObject);
                $scope.pushCatelories(myObject);
            }
        }

        $scope.pushCatelories = function (product) {
            for (var i = 0; i < $scope.catelories.length; i++) {
                if ($scope.catelories[i].ID == product.TypeID) {
                    return;
                }
            }

            var newBranch = {};
            newBranch.ID = product.TypeID;
            newBranch.Name = product.TypeName;
            $scope.catelories.push(newBranch);
        }

        $scope.showAllProduct = function () {
            for (var i = 0; i < $scope.products.length; i++) {
                $scope.products[i].isShow = true;
            }
        }

        $scope.showItemInCatelory = function (id) {
            for (var i = 0; i < $scope.products.length; i++) {
                if ($scope.products[i].TypeID == id) {
                    $scope.products[i].isShow = true;
                }
                else {
                    $scope.products[i].isShow = false;
                }
            }
        }

        $scope.initBannerImage = function (list) {
            var tempLink = "";
            for (var i = 0; i < list.length; i++) {
                if (list[i] != ';' || i == list.length - 1) {
                    tempLink += list[i];
                }
                else {
                    var myObject = {};
                    myObject.link = tempLink;
                    myObject.number = $scope.myCount;

                    tempLink = "";
                    $scope.BannerImages.push(myObject);

                    $scope.myCount += 1;
                }
            }
        }

        $scope.processProductInfor = function () {
            // process color
            var arr = $scope.product.color.split(';');
            for (var i = 0; i < arr.length; i++) {
                var color = {};
                color.index = i;
                color.name = arr[i];
                $scope.listColors.push(color);
            }

            // process size
            $scope.product.size = $scope.product.size.substring(1, $scope.product.size.length);

            var arrszie = $scope.product.size.split(' ');
            for (var i = 0; i < arrszie.length; i++) {
                var color = {};
                color.index = i;
                color.name = arrszie[i];
                $scope.listSize.push(color);
            }
        }

        $scope.hoverimageproduct = function (img) {
            $scope.first_img = img;
        }

        $scope.showproductsdetail = function () {
            $scope.tempProducts = [];
            for (var i = 0; i < $scope.soluong; i++) {
                var item = {};
                item.soluong = 1;
                item.color = "";
                item.size = "";
                item.idInList = $scope.id + '_' + i;
                $scope.tempProducts.push(item);
            }
        }

        $scope.removefromTempList = function (id_temp) {
            for (var i = 0; i < $scope.tempProducts.length; i++) {
                if ($scope.tempProducts[i].idInList == id_temp) {
                    $scope.tempProducts.splice(i, 1);
                    break;
                }
            }
        }

        $scope.themvaogiohang = function () {
            $scope.product.number = $scope.soluong;
            cart_service.add_product_to_cart($scope.product);
            $rootScope.$broadcast('cart_index_controller::recount_amount');
            alert("Đã thêm vào giỏ hàng");
        }

        $scope.themnhieuvaogiohang = function () {

            // validate
            for (var i = 0; i < $scope.tempProducts.length; i++) {
                if ($scope.tempProducts[i].size.trim() == "") {
                    $scope.modal_error_message = "Bạn điền thiếu Kích thước, vui lòng kiểm tra lại.";
                    return;
                }

                if ($scope.tempProducts[i].color.trim() == "") {
                    $scope.modal_error_message = "Bạn điền thiếu Màu sắc, vui lòng kiểm tra lại.";
                    return;
                }
            }

            $scope.product.number = $scope.soluong;
            cart_service.add_list_product_to_cart($scope.product, $scope.tempProducts);

            alert("Đã thêm vào giỏ hàng");
            $('#myModal').modal('hide');
        }

        $scope.showimage = function (link) {
            $('#_imagezoommdal_image').attr("src", link);
            $("#_imagezoommodal").modal("show");
        }
    }]);
