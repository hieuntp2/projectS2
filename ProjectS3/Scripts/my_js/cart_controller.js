
app.controller('cart_controller', ['$scope', '$rootScope', '$http', '$window', 'my_function_services', 'cart_service',
    function ($scope, $rootScope, $http, $window, my_function_services, cart_service) {
        $scope.products = [];

        $scope.init = function () {
            $scope.products = cart_service.getcart();
        }

        $scope.checkoutcart = function () {
            $window.location.href = "/Cart"
        }

        $scope.changeitem = function (id) {
            $rootScope.$broadcast('them_gio_hang_modal_controller::show', id);
        }
        cart_service.loadcart();
        $scope.init();
    }]);

