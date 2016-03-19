app.controller('cart_xacnhan_controller', ['$scope', '$rootScope', '$http', '$window', 'my_function_services', 'cart_service',
    function ($scope, $rootScope, $http, $window, my_function_services, cart_service) {
        $scope.products = [];
        $scope.totalamount = 0;
        $scope.diachi = "";
        $scope.dienthoai = "";
        $scope.thoigian = "";
        $scope.chitiets = [];
        $scope.accecttoken = "";
        $scope.validaterobot = "";
        $scope.hoten = "";
        $scope.isProcessing = false;
        $scope.email = "";

        $scope.init = function (diachi, dienthoai, thoigian, hoten, email) {
            cart_service.init();
            cart_service.remove_double_item();
            $scope.products = cart_service.getcart();
            $scope.counttotalamount();

            $scope.diachi = diachi,
            $scope.dienthoai = dienthoai,
            $scope.thoigian = thoigian;
            $scope.hoten = hoten;
            $scope.email = email;

            var form = $('#__AjaxAntiForgeryForm');
            $scope.accecttoken = $('input[name="__RequestVerificationToken"]', form).val();
            $scope.converttochiviets();
        }

        $scope.counttotalamount = function () {
            $scope.products = cart_service.getcart();
            $scope.totalamount = 0;
            for (var i = 0; i < $scope.products.length; i++) {
                $scope.totalamount += $scope.products[i].price * $scope.products[i].number;
            }
        }

        $scope.chooseproduct = function (id) {
            $rootScope.$broadcast('them_gio_hang_modal_controller::show', id);
        }

        $scope.getaccokent = function () {
            var token = angular.element("input[name='__RequestVerificationToken']").val();

            var config = {
                headers: {
                    '__RequestVerificationToken': token
                }
            };
            return config;
        }

        $scope.xacnhan = function () {

            if ($scope.products.length >= 1) {
                var config = $scope.getaccokent();
                var response = grecaptcha.getResponse();
                if(response.length == 0)
                {
                    $scope.captchaAlert = "Bạn cần xác nhận mình không phải là robot!";
                    return;
                }

                $scope.isProcessing = true;

                $http.post('../../cart/xacnhan',
                      {
                          diachi: $scope.diachi,
                          dienthoai: $scope.dienthoai,
                          thoigian: $scope.thoigian,
                          chitiet: $scope.chitiets,
                          hoten: $scope.hoten,
                          email: $scope.email,
                          captval: response
                      }, config
                  ).
                success(function (data, status, headers, config) {
                    if (data != null) {
                        cart_service.clean();
                        $window.location.href = "../cart/thankyou?ID=" + data;
                    }
                    else {
                        $scope.isProcessing = false;
                        alert("Lỗi khi xác nhận mua hàng!");
                    }
                }).
                error(function (data, status, headers, config) {
                    $scope.isProcessing = false;
                    alert("Lỗi khi xác nhận mua hàng!")
                });
            }
            else {
                $scope.isProcessing = false;
                alert("Giỏ hàng trống! Bạn cần chọn sản phẩm trước");
            }
        }

        $scope.converttochiviets = function () {
            $scope.chitiets = [];
            for (var i = 0; i < $scope.products.length; i++) {
                var item = {};
                item.id = $scope.products[i].id;
                item.soluong = $scope.products[i].number;
                item.color = $scope.products[i].color;
                item.size = $scope.products[i].size;
                item.dongia = $scope.products[i].price;

                item.idbosanpham = $scope.products[i].idbosanpham;
                $scope.chitiets.push(item);
            }
        }

        $scope.back = function () {
            $window.location.href = "/home";
        }

        $rootScope.$on('cart_index_controller::recount_amount', function () {
            $scope.counttotalamount();
        })

        $scope.AddAntiForgeryToken = function (data) {
            data.__RequestVerificationToken = $('#__AjaxAntiForgeryForm input[name=__RequestVerificationToken]').val();
            return data;
        };
    }]);