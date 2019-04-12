app.service('myService', function ($http) {
    this.getProducts = function () {
        return $http.get("/Products/GetAll");
    }

    this.getComments = function () {
        return $http.get("/Yorums/Idyegore", {
            
        })
    }


    this.idYorum = function (product) {
        return $http.get("/Yorums/Idyegore", {

                params: JSON.stringify(product.ProductID),
                dataType: "json"
        })

    }
    this.getbyID = function (ProID) {
        var response = $http({
            method: "post",
            url: "/Products/getbyID",
            params: {
                id: JSON.stringify(ProID)
            }
        });
        return response;
    }

    this.Updatepro = function (product) {
        var response = $http({
            method: "post",
            url: "/Products/Edit",
            data: JSON.stringify(product),
            dataType: "json"
        });
        return response;
    }

    this.DeleteID = function (product) {
        var response = $http({
            method: "POST",
            url: "/Products/DeleteConfirmed",
            dataType: "json",
            data: JSON.stringify(product)
        });
        return response;
    }
});