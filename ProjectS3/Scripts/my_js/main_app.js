var app = angular.module("main_app", []);

app.service('my_function_services', function ($http) {

    var pasreimg = function (text) {
        var count = 0;
        var imgs = [];
        var item = {};
        item.link = "";
        for (var i = 0; i < text.length; i++) {
            if (text[i] != ';') {
                item.link = item.link + text[i];
            }
            else {
                item.number = count;
                item.link = item.link.trim();
                imgs.push(item);
                item = {};
                item.link = "";
                count += 1;
            }
        }
        item.number = count;
        imgs.push(item);
        return imgs;
    }
    var getproductinfo = function (id) {
        var item = {};
        $http.get("../../home/getproductinfor?id=" + id).success(function (data) {
            if (data) {
                item.name = data.Ten;
                item.price = data.DonGia;
                item.detail = data.MoTa;
                item.number = 1;
                item.imgs = pasreimg(data.linkanh);
                item.description = data.MoTa;
                item.id = data.ID;
                item.firstimg = pasreimg(data.linkanh)[0].link;
            }
        })
           .error(function () {
           })

        return item;
    }

    var convertProductListImages = function (mystring) {
        var list = {};
        list = mystring.split(';');
        return list;
    }


    return {
        pasreimg: pasreimg,
        getproductinfo: getproductinfo
    }
})

app.service('cart_service', function ($window) {

    var cart = [];
    var lastupdate;

    var count_number = 0;

    var add_product_to_cart = function (product, idbosanpham, tenbosanpham) {
        if (idbosanpham) {
            product.idbosanpham = idbosanpham;
            product.comment = tenbosanpham;
        }
        else {
            product.comment = "Sản phẩm mua đơn";
            product.idbosanpham = -1;
        }
        loadcart();

        var item = {};
        item.id = product.id;
        item.count_number;
        item.imgs = product.imgs;
        item.name = product.name;
        item.number = product.number;
        item.idbosanpham = product.idbosanpham;
        item.comment = product.comment;
        item.price = product.price;

        item.count_number = count_number + 1;
        count_number += 1;

        var currenttime = new Date();
        lastupdate = currenttime;
        alert(lastupdate);
        cart.push(item);
        save_cart();
    }

    var add_list_product_to_cart = function (product, listdetail) {
        product.comment = "Sản phẩm mua đơn";
        product.idbosanpham = -1;

        loadcart();
        for (var i = 0; i < listdetail.length; i++) {
            var item = {};
            item.id = product.id;
            item.count_number;
            item.imgs = product.imgs;
            item.name = product.name;
            
            item.idbosanpham = product.idbosanpham;
            item.comment = product.comment;
            item.price = product.price;

            item.color = listdetail[i].color;
            item.size = listdetail[i].size;
            item.number = listdetail[i].soluong;

            item.count_number = count_number + 1;
            count_number += 1;
            cart.push(item);
        }
        var currenttime = new Date();
        lastupdate = currenttime;
        save_cart();        
    }

    var save_cart = function () {
        localStorage.setItem("cart", JSON.stringify(cart));
        localStorage.setItem("lastupdate", lastupdate);
    }

    var loadcart = function () {
        cart = JSON.parse(localStorage.getItem("cart"));
        lastupdate = localStorage.getItem("lastupdate");
        remove_double_item();
    }

    var init = function () {
        var now = new Date();
        lastupdate = new Date(localStorage.getItem("lastupdate"));

        var diff = Math.abs(now.getTime() - lastupdate.getTime()) / 3600000;
        if (diff < 18) {
            loadcart();
        }
        else {
            // Nếu lâu hơn 1 giờ thì xóa các sản phẩm trong bộ nhớ tạm
            clean();
        }       
    }

    var clean = function () {
        cart = [];
        $window.localStorage.clear();
    }

    var remove_double_item = function () {
        var testid = [];
        if (cart) {
            count_number = cart.length;
        }
        else {
            cart = [];
            count_number = 0;
        }
    }

    var getcart = function () {
        return cart;
    }

    var removeproduct = function (count_number) {
        if (cart) {
            for (var i = 0; i < cart.length; i++) {
                if (cart[i].count_number == count_number) {
                    cart.splice(i, 1);
                }
            }
        }
        save_cart();
    }

    var getitemincart = function (id) {
        for (var i = 0; i < cart.length; i++) {
            if (cart[i].id == id) {
                return cart[i];
            }
        }
        return null;
    }

    var getLastUpdate = function ()
    {
        lastupdate = localStorage.getItem("lastupdate");
        return lastupdate;
    }

    return {
        add_product_to_cart: add_product_to_cart,
        init: init,
        loadcart: loadcart,
        getcart: getcart,
        clean: clean,
        remove_double_item: remove_double_item,
        removeproduct: removeproduct,
        getitemincart: getitemincart,
        save_cart: save_cart,
        add_list_product_to_cart: add_list_product_to_cart,
        getLastUpdate: getLastUpdate
    }
})

app.directive('ncgRequestVerificationToken', ['$http', function ($http) {
    return function (scope, element, attrs) {
        $http.defaults.headers.common['RequestVerificationToken'] = attrs.ncgRequestVerificationToken || "no request verification token";
    };
}]);