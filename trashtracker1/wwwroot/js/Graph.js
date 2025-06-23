let chartInstance = null;

window.renderLineChart = (labels, data, title, confidence) => {
    const ctx = document.getElementById('lineChart')?.getContext('2d');
    if (!ctx) {
        console.warn("⚠️ Canvas element not found");
        return;
    }

    if (chartInstance) {
        chartInstance.destroy();
    }

    chartInstance = new Chart(ctx, {
        type: 'line',
        data: {
            labels: labels,
            datasets: [{
                label: title,
                data: data,
                borderColor: 'rgb(75, 192, 192)',
                backgroundColor: 'rgba(75, 192, 192, 0.2)',
                tension: 0.4,
                fill: true
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            plugins: {
                tooltip: {
                    callbacks: {
                        label: function(context) {
                            const index = context.dataIndex;
                            const value = context.parsed.y;
                            const confidenceLabel = confidence && confidence[index] != null
                                ? ` (Vertrouwen: ${confidence[index]}%)`
                                : '';
                            return `${title}: ${value}${confidenceLabel}`;
                        }
                    }
                },
                title: {
                    display: true,
                    text: title
                }
            },
            scales: {
                y: {
                    beginAtZero: true
                }
            }
        }
    });
};

window.updateLineChart = (labels, data, title, confidence) => {
    if (!chartInstance) return;

    chartInstance.data.labels = labels;
    chartInstance.data.datasets[0].data = data;
    chartInstance.data.datasets[0].label = title;

    // Update tooltip callback for new confidence values
    chartInstance.options.plugins.tooltip.callbacks.label = function (context) {
        const index = context.dataIndex;
        const value = context.parsed.y;
        const confidenceLabel = confidence && confidence[index] != null
            ? ` (Vertrouwen: ${confidence[index]}%)`
            : '';
        return `${title}: ${value}${confidenceLabel}`;
    };

    chartInstance.options.plugins.title.text = title;
    chartInstance.update();
};
