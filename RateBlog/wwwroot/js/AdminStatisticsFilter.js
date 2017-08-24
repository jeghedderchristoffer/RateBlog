function initialiseStatisticsFilter(InfluenterId, AgeToDisplay, PlatformToDisplay, GenderToDisplay) {

    google.charts.load('current', { 'packages': ['corechart'] });
    google.charts.setOnLoadCallback(drawChart);

    var chart1PlatformSelectedItems = [{ row: 0 }, { row: 1 }, { row: 2 }, { row: 3 }, { row: 4 }, { row: 5 }, { row: 6 }];
    var chart2AgeSelectedItems = [{ row: 0 }, { row: 1 }, { row: 2 }, { row: 3 }, { row: 4 }];
    var chart3GenderSelectedItems = [{ row: 0 }, { row: 1 }];

    var gender = "both";
    var chart4RatingColums;
    var data4RatingColums;
    var option4RatingColums;
    
    function drawChart() {
        InitAgeChart();
        InitPlatformChart();
        InitGenderChart();
        InitColumnChart();
        FilterUpdated();
    }

    function InitColumnChart() {

        data4RatingColums = google.visualization.arrayToDataTable([
            ['Rating', 'Stars', { role: 'style' }],
            ['kvalitet', 1, 'green'],            // RGB value
            ['Troværdighed', 1, 'blue'],            // English color name
            ['Interaktion', 1, 'purple'],
            ['Opførsel', 1, 'orange'],
        ]);

        options4 = {
            vAxis: {
                viewWindowMode: 'explicit',
                viewWindow: {
                    max: 5,
                    min: 0
                }
            },

            title: "Feedback over stars",
            width: 600,
            height: 400,
            bar: { groupWidth: "95%" },
            legend: { position: "none" },
        };

        chart4RatingColums = new google.visualization.ColumnChart(document.getElementById('ChartAvgRatings'));
        chart4RatingColums.draw(data4RatingColums, options4);
    }

    function InitAgeChart() {
        var data2AgeGroup = google.visualization.arrayToDataTable([
            ['Alder', 'AlderSGrup'],
            ['13-18', parseInt(AgeToDisplay[0] + "")],
            ['19-24', parseInt(AgeToDisplay[1] + "")],
            ['25-30', parseInt(AgeToDisplay[2] + "")],
            ['31-39', parseInt(AgeToDisplay[3] + "")],
            ['40+', parseInt(AgeToDisplay[4] + "")]
        ]);

        var options2AgeGroup = {
            title: 'Alder',
            pieSliceText: 'procent'
        };

        var chart2AgeGroup = new google.visualization.PieChart(document.getElementById('piechart2AgeGroup'));

        function selectHandler2AgeGroup() {
            var selectedItem = chart2AgeGroup.getSelection()[0];
            if (selectedItem) {
                chart2AgeSelectedItems.push(selectedItem);
            }
            else {
                chart2AgeSelectedItems = [{row:null}];
            }
            chart2AgeGroup.setSelection(chart2AgeSelectedItems);
            FilterUpdated();
        }

        google.visualization.events.addListener(chart2AgeGroup, 'select', selectHandler2AgeGroup);
        chart2AgeGroup.draw(data2AgeGroup, options2AgeGroup);
        chart2AgeGroup.setSelection(chart2AgeSelectedItems);
 
    }

    function InitGenderChart() {

        var data3Gender = google.visualization.arrayToDataTable([
            ['Køn', 'Procentage'],
            ['Male ♂', parseInt(GenderToDisplay[0])],
            ['Female ♀', parseInt(GenderToDisplay[1])],
        ]);

        var options3Gender = {
            title: 'Køn',
            pieSliceText: 'procent',
        };

        var chart3Gender = new google.visualization.PieChart(document.getElementById('piechart3Gender'));

        function selectHandler3Gender() {
            var selectedItem = chart3Gender.getSelection()[0];
            if (selectedItem) {
                chart3GenderSelectedItems.push(selectedItem);
            }
            else {
                chart3GenderSelectedItems = [{row:null }];
            }
            chart3Gender.setSelection(chart3GenderSelectedItems);
            FilterUpdated();
        }

        google.visualization.events.addListener(chart3Gender, 'select', selectHandler3Gender);
        chart3Gender.draw(data3Gender, options3Gender);
        chart3Gender.setSelection(chart3GenderSelectedItems);
    }

    function InitPlatformChart() {
        var data1Platform = google.visualization.arrayToDataTable([
            ['Platform', 'Size'],
            ['Twitter', parseInt(PlatformToDisplay[0]+"")],
            ['Youtube', parseInt(PlatformToDisplay[1]+"")],
            ['Twitch', parseInt(PlatformToDisplay[2]+"")],
            ['Instagram ', parseInt(PlatformToDisplay[3]+"")],
            ['Snapchat ', parseInt(PlatformToDisplay[4]+"")],
            ['Facebook', parseInt(PlatformToDisplay[5]+"")],
            ['Website ', parseInt(PlatformToDisplay[6]+"")]
        ]);

        var options1Platform = {
            title: 'Platform',
            pieSliceText: 'procent',
        };

        var chart1Platform = new google.visualization.PieChart(document.getElementById('piechart1Platform'));

        function selectHandler1Platform() {
            var selectedItem = chart1Platform.getSelection()[0];
            if (selectedItem) {
                chart1PlatformSelectedItems.push(selectedItem);
            }
            else {
                chart1PlatformSelectedItems = [{row:null}];
            }
            chart1Platform.setSelection(chart1PlatformSelectedItems);
            FilterUpdated();
        }
        google.visualization.events.addListener(chart1Platform, 'select', selectHandler1Platform);
        chart1Platform.draw(data1Platform, options1Platform);
        chart1Platform.setSelection(chart1PlatformSelectedItems);

    }

    function FilterUpdated() {
        //Convert And Post W Ajax
        var ItemFormatId = InfluenterId;
        var ItemFormatPlatform = new Array();
        var ItemFormatAgeGroup = new Array();
        var ItemFormatGender = new Array();

        for (var i = 0; i < chart1PlatformSelectedItems.length; i++) {
            ItemFormatPlatform[i] = chart1PlatformSelectedItems[i].row;
        }
        for (var i = 0; i < chart2AgeSelectedItems.length; i++) {
            ItemFormatAgeGroup[i] = chart2AgeSelectedItems[i].row;
        }


        for (var i = 0; i < chart3GenderSelectedItems.length; i++) {
            if (chart3GenderSelectedItems[i].row == 0) {
                gender = "male";
            }
            if (chart3GenderSelectedItems[i].row == 1) {
                gender = "female";
            }
            if (chart3GenderSelectedItems[i].row == 1) {
                if (chart3GenderSelectedItems[i - 1].row == 0) {
                    gender = "both";
                }
            }
            if (chart3GenderSelectedItems.length == 3) {
                gender = "both";
            }
            if (chart3GenderSelectedItems[i].row == null) {
                gender = "none";
            }
        }

        var data = {
            Id: ItemFormatId,
            Platform: ItemFormatPlatform,
            AgeGroup: ItemFormatAgeGroup,
            Gender: gender
        };

        $.ajax({
            url: "/Admin/PostFilter",
            type: "POST",
            traditional: true,
            data: data,
            success: function (newObject) {

                document.getElementById("NumberUsers").innerHTML = " " + newObject.filterdNumberOfFeedbacks + " (" + newObject.filterdNumberOfUsers + ")";
                document.getElementById("NumberTotalScore").innerHTML = Math.round(((newObject.filterdKvalitet + newObject.filterdInteraktion + newObject.filterdOpførsel + newObject.filterdTroværdighed) / 4) * 10) / 10;
                document.getElementById("NumberNps").innerHTML = " " + newObject.filterdNps + "%";

                data4RatingColums = google.visualization.arrayToDataTable([
                    ['Rating', 'Stars', { role: 'style' }],
                    ['kvalitet', newObject.filterdKvalitet, 'green'],            // RGB value
                    ['Troværdighed', newObject.filterdTroværdighed, 'blue'],            // English color name
                    ['Interaktion', newObject.filterdInteraktion, 'purple'],
                    ['Opførsel', newObject.filterdOpførsel, 'orange'],
                ]);

                options4 = {
                    vAxis: {
                        viewWindowMode: 'explicit',
                        viewWindow: {
                            max: 5,
                            min: 0
                        }
                    },

                    title: "Feedback over stars",
                    width: 600,
                    height: 400,
                    bar: { groupWidth: "95%" },
                    legend: { position: "none" },
                };

                chart4RatingColums = new google.visualization.ColumnChart(document.getElementById('ChartAvgRatings'));
                chart4RatingColums.draw(data4RatingColums, options4);
            }
        });
    }

}