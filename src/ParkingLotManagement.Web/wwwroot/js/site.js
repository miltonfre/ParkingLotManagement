

$(document).ready(function () {   
    OnGetHourlyFeeAsync();
    OnGetCapacitySpotsAsync();  
    OnGetAvailableSpotsAsync('#availableSpost');
    OnGetCurrentParkedCarsAsync();
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

        Post(url, success, error) {
            $.ajax({
                type: "POST",
                url: url,
                /*headers: { "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val() }, */
               /* body: JSON.stringify(data),*/
                success: success,
                error: error
            });
        }
    }
}
const api = ApiService();

const OnError = (message) => {
   
    $('#alert').text(message);
    $('#alert').removeAttr("class");
    $('#alert').addClass("alert alert-danger");
    setTimeout(function () {
        $('#alert').alert('dispose');
    }, 5000);
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
const showSuccessAlert = (data) => {
    $('#alert').text(data);
    $('#alert').removeAttr("class");
    $('#alert').addClass("alert alert-success");
}


const OnPostCarInAsync = () => {
    var tagNumber = $('#tagNumber').val();
    if (tagNumber === "") {
        OnError("Tag number is required");
        return;
    }
    let body = { tagNumber: tagNumber };
    api.Get("/?handler=CarIn&tagNumber=" + tagNumber
        , (data) => {
            OnGetAvailableSpotsAsync('#availableSpost');
            OnGetCurrentParkedCarsAsync();
            $('#tagNumber').val('');
            showSuccessAlert(data);
        }
        , (response) => { OnError(response.responseText) });
    
}
const OnPostCarOutAsync = () => {
    var tagNumber = $('#tagNumber').val();
    if (tagNumber === "") {
        OnError("Tag number is required");
        return;
    }
      
    let data = { tagNumber: tagNumber };
    api.Get("/?handler=CarOut&tagNumber=" + tagNumber
        , (data) => {
            OnGetAvailableSpotsAsync('#availableSpost');
            OnGetCurrentParkedCarsAsync();
            $('#tagNumber').val('');
            showSuccessAlert(data);
            }
        , (response) => { OnError(response.responseText) });
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
    var table = $('#parkedCars').dataTable({
        "aaData": data.data,
        "bDestroy": true,
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