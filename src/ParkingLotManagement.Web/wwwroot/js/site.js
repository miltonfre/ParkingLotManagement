

$(document).ready(function () {
    OnGetCurrentParkedCarsAsync();
    OnGetHourlyFeeAsync();
    OnGetCapacitySpotsAsync();  
    OnGetAvailableSpotsAsync('#availableSpost');
});

const ApiService = () => {
    return {
        Get(url, success, error) {
            $.ajax({
                type: "GET",
                url: url,
                success: success,
                error: error
            });
        },

        Post(url, data, success, error) {
            $.ajax({
                type: "POST",
                url: url,
                data: data,
                success: success,
                error: error
            });
        }
    }
}
const api = ApiService();

const OnError = (message) => {
    alert(message);
}

const showModal = () => {
    $('#form-modal').modal('show');
    OnGetTotalRevenueTodayAsync();
    OnGetAverageCarsPerDayAsync();
    OnGetAverageRevenuePerDayAsync();
    OnGetAvailableSpotsAsync('#availableSpotsModal');
}
const closeModal = () => {
    $('#form-modal').modal('hide');
}

const OnGetAvailableSpotsAsync = (id) => {
    api.Get("/?handler=AvailableSpots"
        , (data) => { $(id).text(data); }
        , OnError);
}


const OnGetCurrentParkedCarsAsync = () => {
    api.Get("/?handler=CurrentParkedCars"
        , (data) => { $("#spotsTaken").text(data.data.length); fillDataTable(data); }
        , OnError);
}
const OnGetHourlyFeeAsync = () => {
    api.Get("/?handler=HourlyFee"
        , (data) => { $("#hourlyFee").text(data);}
        , OnError);
}
const OnGetCapacitySpotsAsync = () => {
    api.Get("/?handler=CapacitySpots"
        , (data) => { $("#capacitySpots").text(data); }
        , OnError);
}
const OnGetTotalRevenueTodayAsync = () => {
    api.Get("/?handler=TotalRevenueToday"
        , (data) => { $("#totalRevenueToday").text(Math.round(data * 100) / 100); }
        , OnError);
}
const OnGetAverageCarsPerDayAsync = () => {
    api.Get("/?handler=AverageCarsPerDay"
        , (data) => { $("#averageCarsPerDay").text(Math.round(data * 100) / 100 ); }
        , OnError);
}
const OnGetAverageRevenuePerDayAsync = () => {
    api.Get("/?handler=AverageRevenuePerDay"
        , (data) => { $("#averageRevenuePerDay").text(Math.round(data * 100) / 100); }
        , OnError);
}

const fillDataTable = (data) => {
    $('#parkedCars').dataTable({
        "aaData": data.data,
        columns: [{
            data: "tagNumber"
        }, {
            data: "entryTime",
            render: DataTable.render.datetime('MM-DD-YYYY HH:mm a')
        }, {
            data: "elapsedTime"
        }]
    })
}

