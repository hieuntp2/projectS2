
app.controller('addgroupproduct_controller', ['$scope', '$rootScope', '$http', '$window', 'my_function_services', 'cart_service',
    function ($scope, $rootScope, $http, $window, my_function_services, cart_service) {
        $scope.Ten = "";
        $scope.Mota = "";
        $scope.products = [];
        $scope.allproduct = [];
        $scope.Tenmessage = "";
        $scope.MoTaMessage = "";
        $scope.SanPhamMessage = "";

        $scope.addproduct = function (product) {

            if (!$scope.isHaveProdcut(product.id)) {
                product.number = 0;
                $scope.products.push(product);
            }
        }

        $scope.removeproduct = function (id) {
            for (var i = 0; i < $scope.products.length; i++) {
                if ($scope.products[i].id == id) {
                    $scope.products.splice(i, 1);
                }
            }
        }

        $scope.isHaveProdcut = function (id) {
            for (var i = 0; i < $scope.products.length; i++) {
                if ($scope.products[i].id == id) {
                    return true;
                }
            }
            return false;
        }

        $scope.showmodal_allproduct = function () {
            if ($scope.allproduct.length >= 1) {

            }
            else {
                $http.get("../../home/getallproducts").success(function (data) {
                    for (var i = 0; i < data.length; i++) {
                        var item = {};
                        item.name = data[i].Ten;
                        item.price = data[i].DonGia;
                        item.id = data[i].ID;

                        $scope.allproduct.push(item);
                    }
                })
            .error(function () {
                alert("Lỗi!");
            })
            }

            $("#add_group_product_modal").modal('show');

        }

        $scope.check_before_submit = function () {

            $scope.Tenmessage = "";
            $scope.MoTaMessage = "";
            $scope.SanPhamMessage = "";


            if (!$scope.Ten)
            {
                $scope.Tenmessage = "Cần nhập Tên bộ sản phẩm";
                return false;
            }

            if (!$scope.Mota)
            {
                $scope.MoTaMessage = "Cần nhập Mô Tả bộ sản phẩm";
                return false;
            }

            if ($scope.products.length <= 0) {
                $scope.SanPhamMessage = "Chưa có sản phẩm nào trong bộ sản phẩm";
                return false;
            }

            for (var i = 0; i < $scope.products.length; i++) {
                if ($scope.products[i].number == 0) {
                    alert("Cần nhập số lượng cho sản phẩm mã: " + $scope.products[i].id);
                    return false;
                }
            }
            return true;
        }

        $scope.finish = function () {

            if ($scope.check_before_submit()) {
                var data = {};
                data.Ten = $scope.Ten;
                data.Mota = $scope.Mota;
                data.products = $scope.products;

                // Simple POST request example (passing data) :
                $http.post('../../admin/addgroupproduct', { model: data }).
                  success(function (data, status, headers, config) {
                      $window.location.href = "../../admin";
                  }).
                  error(function (data, status, headers, config) {
                      alert("Lỗi khi đăng tải");
                  });
            }
        }
    }]);