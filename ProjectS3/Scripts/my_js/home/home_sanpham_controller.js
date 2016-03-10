﻿app.controller('home_bosanpham_controller', ['$scope', '$scope', '$rootScope', '$http', 'my_function_services', 'cart_service',
    function ($scope, $scope, $rootScope, $http, my_function_services, cart_service) {

        $scope.id = "";
        $scope.name = "";
        $scope.price = "";
        $scope.imgs = "";
        $scope.product = {};
        $scope.soluong = 1;
        $scope.first_img = "";
        $scope.product.detail = "";
        $scope.tempProducts = [];
        $scope.listColors = [];
        $scope.listSize = [];
        $scope.modal_error_message = "";
        $scope.listproductimage = {};
        $scope.init = function (sanpham) {
            cart_service.init();
            $scope.id = sanpham.ID;
            $scope.name = sanpham.Ten;
            $scope.price = sanpham.DonGia;
            $scope.imgs = my_function_services.pasreimg(sanpham.linkanh);
            $scope.first_img = $scope.imgs[0];
            $scope.product.imgs = $scope.imgs;
            $scope.product.name = $scope.name;
            $scope.product.number = $scope.soluong;
            $scope.product.price = $scope.price;
            $scope.product.color = sanpham.color;
            $scope.product.size = sanpham.size;
            $scope.product.detail = sanpham.MoTa;
            $scope.product.id = sanpham.ID;
            $scope.isStock = sanpham.isInstock;

            $scope.processProductInfor();
        }

        $scope.processProductInfor = function()
        {
            // process color
            var arr = $scope.product.color.split(';');
            for (var i = 0; i < arr.length; i++)
            {
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

        $scope.showproductsdetail = function()
        {
            $scope.tempProducts = [];
            for (var i = 0; i < $scope.soluong; i++)
            {
                var item = {};
                item.soluong = 1;
                item.color = "";
                item.size = "";
                item.idInList = $scope.id + '_' + i;
                $scope.tempProducts.push(item);
            }
        }

        $scope.removefromTempList = function (id_temp)
        {
            for(var i = 0; i < $scope.tempProducts.length; i++)
            {
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
            for (var i = 0; i < $scope.tempProducts.length; i++)
            {
                if ($scope.tempProducts[i].size.trim() == "")
                {
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
