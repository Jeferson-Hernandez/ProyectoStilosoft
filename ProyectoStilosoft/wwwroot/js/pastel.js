Highcharts.chart('container', {
    chart: {
        plotBackgroundColor: null,
        plotBorderWidth: null,
        plotShadow: false,
        type: 'pie'
    },
    title: {
        text: 'Servicios realizados en el 2022'
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
        data: [{
            name: 'Corte Cabello Hombre',
            y: 61.41,
            sliced: true,
            selected: true
        }, {
            name: 'Alizado',
            y: 11.84
        }, {
            name: 'Rayito Mujer',
            y: 10.85
        }, {
            name: 'balayage',
            y: 4.67
        }, {
            name: 'Tinturado',
            y: 4.18        
        }]
    }]
});