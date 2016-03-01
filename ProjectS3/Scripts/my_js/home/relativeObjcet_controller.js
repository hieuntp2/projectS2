app.controller('relatedObject_controller', ['$scope', '$scope', '$rootScope', '$http', 'my_function_services', 'cart_service',
    function ($scope, $scope, $rootScope, $http, my_function_services, cart_service) {

        $scope.items = [];
        $scope.init = function (id, isgroup, israndom) {
            $http.get("../../home/listsanphamrelated?id=" + id + "&&bsp=" + isgroup + "&&israndom="+israndom).success(function (data) {
                for (var i = 0; i < data.length; i++) {
                    $scope.additem(data[i]);
                }
            })
            .error(function () {
                alert("Lỗi!");
            })          
        }

        $scope.additem = function (sanpham) {
            var item = {};
            item.img = my_function_services.pasreimg(sanpham.linkanh)[0].link;
            item.name = sanpham.name;
            item.id = sanpham.id;
            item.isgroup = sanpham.isgroup;

            if (item.isgroup)
            {
                item.hreft = "../../home/bosanpham/" + item.id;
            }
            else {
                item.hreft = "../../home/sanpham/" + item.id;
            }

            $scope.items.push(item);
        }

        $scope.xemsanpham = function (hreft) {      
            window.location.href = hreft;
        }

    }]);