app.controller('home_index_controller', ['$scope', '$scope', '$rootScope', '$http', 'my_function_services', 'cart_service',
    function ($scope, $scope, $rootScope, $http, my_function_services, cart_service) {
        $scope.products = [];

        $scope.groupproducts = [];

        $scope.init = function () {
            $http.get("../../home/getallproducts").success(function (data) {
                for (var i = 0; i < data.length; i++) {
                    $scope.addproduct(data[i]);
                }
            })
            .error(function () {
                alert("Lỗi!");
            })

            $http.get("../../home/getallgroupproducts").success(function (data) {
                for (var i = 0; i < data.length; i++) {
                    $scope.addgrouproduct(data[i]);
                }
            })
           .error(function () {
               alert("Lỗi!");
           })
        }

        $scope.addproduct = function (sanpham) {
            var item = {};
            item.img = my_function_services.pasreimg(sanpham.linkanh)[0].link;
            item.name = sanpham.Ten;
            item.price = sanpham.DonGia;
            item.id = sanpham.ID;
            item.description = sanpham.MoTa;
            $scope.products.push(item);
        }

        $scope.addgrouproduct = function (sanpham) {
            var item = {};
            item.img = my_function_services.pasreimg(sanpham.img)[0].link;
            item.imgs = my_function_services.pasreimg(sanpham.img);
            item.name = sanpham.Ten;
            item.id = sanpham.id;
            $scope.groupproducts.push(item);
        }

        $scope.xemsanpham = function (id) {      
            //$rootScope.$broadcast('xem_san_pham::show', id);
            window.location.href = "../../home/sanpham/" + id;
        }

        $scope.xembosanpham = function (id) {
           window.location.href = "../../home/bosanpham/" + id;
           // $rootScope.$broadcast('xem_bo_san_pham::show', id);
        }
    }]);

app.controller('them_gio_hang_modal_controller', ['$scope', '$rootScope', '$http', 'my_function_services', 'cart_service',
    function ($scope, $rootScope, $http, my_function_services, cart_service) {
        $scope.product = {};
        $scope.bosanpham = {};

        $scope.themvaogiohang = function () {
            cart_service.add_product_to_cart($scope.product);
            $rootScope.$broadcast('cart_index_controller::recount_amount');
            $scope.product = {};
        }

        $scope.huy = function () {
            $scope.product = {};
        }

        $rootScope.$on('xem_san_pham::show', function (event, id, number) {

            $scope.product = cart_service.getitemincart(id);
            if ($scope.product == null)
            {
                $scope.product = my_function_services.getproductinfo(id);
            }           

            if (number)
            {
                $scope.product.number = number;
            }
            if ($scope.product) {
                $("#ThemVaoGioHang_Modal").modal('show');
            }
        })

        $rootScope.$on('xem_bo_san_pham::show', function (event, id, number) {

            $scope.product = cart_service.getitemincart(id);
            if ($scope.product == null) {
                $scope.product = my_function_services.getproductinfo(id);
            }

            if (number) {
                $scope.product.number = number;
            }
            if ($scope.product) {
                $("#xembosanpham_modal").modal('show');
            }
        })
    }]);

