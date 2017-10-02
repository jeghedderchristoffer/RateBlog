function drawGenderPie(male,female) {
    var data = google.visualization.arrayToDataTable([
        ['Task', 'Hours per Day'],
        ['♂', male],
        ['♀', female],
    ]);

    var options = {
        chartArea: { width: '100%', height: '80%' },
        width: 300,
        height: 250,
        pieSliceTextStyle: {
            color: 'black',
        },
        pieSliceText: 'label',
        legend: 'none',
        fontSize: '30'
    };

    var chart = new google.visualization.PieChart(document.getElementById('GenderPie'));

    chart.draw(data, options);
}


function drawAgeChart(data1,data2,data3,data4,data5,data6,data7) {
    var data = google.visualization.arrayToDataTable([
        ["Alder", "Antal", { role: "style" }],
        ["13-17", data1, "grey"],
        ["18-24", data2, "blue"],
        ["25-34", data3, "grey"],
        ["35-44", data4, "blue"],
        ["45-54", data5, "grey"],
        ["55-64", data6, "blue"],
        ["65+", data7, "grey"]
    ]);

    var view = new google.visualization.DataView(data);
    view.setColumns([0, 1,
        {
            calc: "stringify",
            sourceColumn: 1,
            type: "string",
            role: "annotation"
        },
        2]);

    var options = {
        pieSliceText: 'procentage',
        width: 400,
        height: 250,
        bar: { groupWidth: "95%" },
        legend: { position: "none" },
    };

    var chart = new google.visualization.BarChart(document.getElementById("AgeChart"));
    chart.draw(view, options);
}


function drawEngagementChart(likes,comments) { 
    var data = google.visualization.arrayToDataTable([
        ['Task', 'Hours per Day'],
        ['', likes],
        ['', comments],

    ]);

    var options = {
        chartArea: { width: '100%', height: '100%' },
        width: 250,
        height: 250,
        pieHole: 0.6,
        pieSliceTextStyle: {
            color: 'white',
        },
        //fontName : 'Arial'
        legend: 'none',
        pieSliceText: 'label',
        fontSize: '14',
        colors: ['#089de3', '#ffa500']
    };

    var chart = new google.visualization.PieChart(document.getElementById('EngagementChart'));
    chart.draw(data, options);
}


function drawMediaPie(Likes,Comments) {
    var data= google.visualization.arrayToDataTable([
        ['Task', 'Hours per Day'],
        ['', Likes],
        ['', Comments],
    ]);

    var options = {
        chartArea: { width: '100%', height: '80%' },
        width: 250,
        height: 200,
        pieHole: 0.9,
        pieSliceTextStyle: {
            color: 'black',
        },
        legend: 'none'
    };

    var chart = new google.visualization.PieChart(document.getElementById('MediaPie'));
    chart.draw(data, options);
}
