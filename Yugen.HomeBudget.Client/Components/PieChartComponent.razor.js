export function newPieChart(elementId, labels, datasets) {
    let element = document.getElementById(elementId).getContext("2d");
    return new Chart(element, {
        type: "doughnut",
        data: {
            labels: labels,
            datasets: datasets,
        },
        options: {
            cutoutPercentage: 60,
            borderColor: "#F3F6F8",
            borderWidth: 15,
            backgroundColor: "#F3F6F8",
            tooltips: {
                backgroundColor: "#F3F6F8",
                titleFontFamily: "Inter",
                titleFontColor: "#8F92A1",
                titleFontSize: 12,
                bodyFontFamily: "Inter",
                bodyFontColor: "#171717",
                bodyFontStyle: "bold",
                bodyFontSize: 16,
                multiKeyBackground: "transparent",
                displayColors: false,
                bodyAlign: "center",
                titleAlign: "center",
                xPadding: 15,
                yPadding: 12,
            },

            title: {
                display: false,
            },
            legend: {
                display: false,
            },

            scales: {
                yAxes: [
                    {
                        gridLines: {
                            display: false,
                            drawTicks: false,
                            drawBorder: false,
                        },
                        ticks: {
                            display: false,
                        },
                    },
                ],
                xAxes: [
                    {
                        gridLines: {
                            display: false,
                            drawBorder: false,
                        },
                        ticks: {
                            display: false,
                        },
                    },
                ],
            },
        },
    });
}