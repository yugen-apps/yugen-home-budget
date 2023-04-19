export function newLineChart(elementId, labels, datasets) {
    let element = document.getElementById(elementId).getContext("2d");
    return new Chart(element, {
        type: "line",
        data: {
            labels: labels,
            datasets: datasets,
        },
        options: {
            tooltips: {
                intersect: false,
                backgroundColor: "#fbfbfb",
                titleFontColor: "#8F92A1",
                titleFontSize: 16,
                titleFontFamily: "Inter",
                titleFontStyle: "400",
                bodyFontFamily: "Inter",
                bodyFontColor: "#171717",
                bodyFontSize: 16,
                multiKeyBackground: "transparent",
                displayColors: false,
                xPadding: 30,
                yPadding: 15,
                borderColor: "rgba(143, 146, 161, .1)",
                borderWidth: 1,
                title: false,
            },
            title: {
                display: false,
            },
            layout: {
                padding: {
                    top: 0,
                },
            },
            legend: false,
            scales: {
                yAxes: [
                    {
                        gridLines: {
                            display: false,
                            drawTicks: false,
                            drawBorder: false,
                        },
                        ticks: {
                            padding: 35,
                            max: 300,
                            min: 50,
                        },
                    },
                ],
                xAxes: [
                    {
                        gridLines: {
                            drawBorder: false,
                            color: "rgba(143, 146, 161, .1)",
                            zeroLineColor: "rgba(143, 146, 161, .1)",
                        },
                        ticks: {
                            padding: 20,
                        },
                    },
                ],
            },
        },
    });
}