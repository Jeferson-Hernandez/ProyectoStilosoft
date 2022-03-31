$(document).ready(function () {
    $.ajax({
        url: '@Url.Action("reporteServicio", "Dashboard")',
        type: "GET",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            graficaPastel(data);
            console.log(data)

            var arrayNombre = [];
            var arrayCantidad = [];

            for (var i = 0; i < data.length; i++) {

                /*console.log(data[i].nombre)*/
                arrayNombre.push(data[i].nombre)
                arrayCantidad.push(data[i].cantidad)
            }

        },
        error: function (error) {
            console.log(error)
        }
    });
});


function graficaPastel(data) {
    Highcharts.chart('container', {
        chart: {
            plotBackgroundColor: null,
            plotBorderWidth: null,
            plotShadow: false,
            type: 'pie'
        },
        title: {
            text: 'Browser market shares in January, 2018'
        },
        tooltip: {
            pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>'
        },
        accessibility: {
            point: {
                valueSuffix: '%'
            }
        },
        plotOptions: {
            pie: {
                allowPointSelect: true,
                cursor: 'pointer',
                dataLabels: {
                    enabled: true,
                    format: '<b>{point.name}</b>: {point.percentage:.1f} %'
                }
            }
        },
        series: [{
            name: 'Brands',
            colorByPoint: true,
            data: [{ name: 'name 1', y: 10 }, { name: 'name 2', y: 5 }]
        }]
        }]
    }); 
}
      

