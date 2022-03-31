$(document).ready(function () {
    $.ajax({
        url: '@Url.Action("reporteServicios", "Dashboard")',
        type: "GET",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (data) {

            /*console.log(data)    */

            var arrayServicio = [];
            var arrayCantidad = [];

            for (var i = 0; i < data.length; i++) {

                /*console.log(data[i].nombre)*/

                arrayServicio.push(data[i].servicio);
                arrayCantidad.push(data[i].cantidad);
                
            }
            DrawChart(data);
        },
        error: function (error) {
            console.log(error)
        }
    });
});


function DrawChart(data) {
    Highcharts.chart('container', {
        chart: {
            plotBackgroundColor: null,
            plotBorderWidth: null,
            plotShadow: false,
            type: 'pie'
        },
        title: {
            text: 'Porcentaje de servicios'
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
            name: 'Datos',
            colorByPoint: true,
            data: data
        }]
    });
}
