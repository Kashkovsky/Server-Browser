var app = angular.module("fileBrowserApp", []);
app.config(["$httpProvider", function ($httpProvider) {
    $httpProvider.interceptors.push('middleware');
}]);

app.factory('middleware', function () {
    return {
        request: function (config) {
            config.url = "../api/browse/" + config.url;
            return config;
        }
    };
});
app.controller("tableCtrl", ["$scope", "$http", function ($scope, $http) {


    function getCurrentSizes(m) {
        var result = { s: 0, r: 0, k: 0 };
        for (var i = 0; i < m.length; i++) {
            if (m[i].ApplicationCategory.Value === 0 || m[i].ApplicationCategory.Value > 8) {
                continue;
            }
            switch (m[i].Size) {
                case 0:
                    result.s++;
                    break;
                case 1:
                    result.r++;
                    break;
                default:
                    result.k++;
                    break;
            }
        }
        return result;
    }

    function getLocation(m) { return (m.Location !== "") ? m.Location : "root"; };

    function getIcon(i) {
        var result = i.ApplicationCategory.Value;
        if (result === null) {
            return "glyphicon glyphicon-list-alt";
        }
        console.log(result);
        switch (result) {
            case 0:
                return "glyphicon glyphicon-folder-open";
            case 1:
                return "glyphicon glyphicon-list-alt";
            case 3:
                return "glyphicon glyphicon-asterisk";
            case 4:
                return "glyphicon glyphicon-headphones";
            case 5:
                return "glyphicon glyphicon-film";
            case 7:
                return "glyphicon glyphicon-picture";
            case 6: case 8:
                return "glyphicon glyphicon-cog";
            case 9: case 10: case 11: case 12:
                return "glyphicon glyphicon-hdd";
            default:
                return "glyphicon glyphicon-list-alt";
        }
    };

    var config = {
        transformResponse: function (response, headers) {
            if (response === "[]") { return null; }
            var items = [];
            var elems = angular.fromJson(response);
            var sizes = getCurrentSizes(elems);
            $scope.sizesCount = {
                small: sizes.s,
                middle: sizes.r,
                big: sizes.k
            };
            for (var i = 0; i < elems.length; i++) {
                var item = elems[i];
                var icon = getIcon(item);
                var location = getLocation(item);
                items.push({
                    name: item.Name,
                    location: location,
                    path: item.Path,
                    parrentPath: item.CurrentParentPath,
                    icon: icon,
                    size: item.Size
                });

            }
            return items;
        }
    };

    function sendRequest(url, httpMethod, callback, item, requestData) {
        $http({
            url: url, method: httpMethod, data: requestData,
            headers: {
                'Content-Type': "application/json"
            }, transformResponse: config.transformResponse
        }, config)
            .success(callback);
    };

    $http.get("", config).success(function (responseData) {
        $scope.items = responseData;
        $scope.dir = responseData[0].location;
    });


    function refreshData(data) {
        $scope.items = data;
        $scope.dir = data[0].location;
    };

    $scope.expandItem = function (item) {
        sendRequest("", "POST", function (responseData) {
            if (responseData === null) {
                $("#alertTemplate").modal("show");
                return;
            }
            refreshData(responseData);
        }, item, { Path: item.path });
    }

    $scope.upStairs = function () {
        sendRequest("upstairs", "POST", function (responseData) {
            refreshData(responseData);
        }, null, { Path: $scope.dir });
    }

    $scope.root = function () {
        sendRequest("upstairs", "POST", function (responseData) {
            refreshData(responseData);
        }, null, { Path: "root" });
    }
}]);