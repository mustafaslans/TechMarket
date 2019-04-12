app.controller('myController', function ($scope, myService) {
    $scope.divProduct = false;
    GetAllPro();
    function GetAllPro() {
        var getData = myService.getProducts();
        var FileStatus;
        getData.then(function (response) {
            $scope.product = response.data;
        }, function () {
            alert("veriler getirilemedi")
        });

    }

    function GetAllYor() {
        var getData = myService.getComments();
        var FileStatus;
        getData.then(function (response) {
            $scope.yorum = response.data;
        }, function () {
            alert("veriler getirilemedi")
        });

    }


    function GetIDYor(produc) {
        var getData = myService.idYorum();
        var FileStatus;
        getData.then(function (response) {
            $scope.yorumid = response.data;
        }, function () {
            alert("veriler getirilemedi")
        });

    }

    $scope.editProduct = function (product) {
        var getData = myService.getbyID(product.ProductId);
        getData.then(function (pro) {
            $scope.product = pro.data;
            $scope.productId = product.ProductId;
            $scope.productMarka = product.Marka;
            $scope.productModel = product.Model;
            $scope.productCategory = product.Category;
            $scope.productPrice = product.Price;
            //$scope.productPhoto = product.Photo;
            $scope.productDescription = product.Description;
            $scope.Action = "Update";
            $scope.divProduct = true;
            GetAllPro();
        });
    }
    $scope.Category = {
        Laptop: 0,
        Telefon: 1,
        Aksesuar: 2
    }
    $scope.showSelectValue = function (item) {
        console.log(item);
        $scope.Secili = item;

    }

    $scope.UpdateProduct = function () {
        var Product = {
            ProductId: $scope.productId,
            Marka: $scope.productMarka,
            Model: $scope.productModel,
            Price: $scope.productPrice,
            Description: $scope.productDescription,
            Category: $scope.Secili,
     
        };
        console.log($scope.Secili)
        var getAction = $scope.Action;
        var getData = myService.Updatepro(Product);
        $scope.divEmployee = false;
        location.reload();
        GetAllPro();
    }

    $scope.DeleteProduct = function (product) {
        var getData = myService.DeleteID(product);
        getData.then(function (response) {
            alert("Silindi")
        });
    }

});
//app.constant('Constants', {
//    Category: {
//        1: 'Laptop',
//        2: 'Telefon',
//        3: 'Aksesuar'
//    }
//});
