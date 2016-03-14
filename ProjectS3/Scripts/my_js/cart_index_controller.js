app.controller('cart_index_controller', ['$scope', '$rootScope', '$http', '$window', 'my_function_services', 'cart_service',
    function ($scope, $rootScope, $http, $window, my_function_services, cart_service) {
        $scope.products = [];
        $scope.totalamount = 0;
        $scope.lastUpdate;

        $scope.init = function () {
            cart_service.init();
            cart_service.remove_double_item();
            $scope.products = cart_service.getcart();
            $scope.counttotalamount();
            $scope.lastUpdate = cart_service.getLastUpdate();
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

        $scope.saveData = function () {
            $scope.counttotalamount();
            cart_service.save_cart();
        }

        $scope.deleteproduct = function (count_number) {
            var quest = confirm("Bạn muốn xóa sản phẩm này?");
            if (quest) {
                cart_service.removeproduct(count_number);
                $scope.counttotalamount();
            }
        }

        $scope.muahang = function () {
            if ($scope.products.length >= 1) {
                $window.location.href = "/cart/thongtin";
            }
            else {
                alert("Giỏ hàng trống! Bạn cần chọn sản phẩm trước");
            }
        }

        $scope.back = function () {
            $window.location.href = "/home";
        }

        $rootScope.$on('cart_index_controller::recount_amount', function () {
            $scope.counttotalamount();
        })


    }]);

