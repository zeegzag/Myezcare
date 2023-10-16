let calendar = null;
let selectedYear = new Date().getFullYear();
var authCodeData = [];
var authCodeStartDate = null;
var authCodeEndDate = null;
var editPopupModal = false;

function editEvent(event) {

    $('#event-modal input[name="event-index"]').val(event ? event.id : '');
    $('#event-modal input[name="nurseScheduleId"]').val(event ? event.nurseScheduleID : '');
    if (event)
        $('#event-modal input[name="event-end-date"]').datepicker('update', event.originalEndDate != null ? event.originalEndDate : event.endDate);

    $("#daily").prop("checked", false);
    $("#weekly").prop("checked", false);
    $("#monthly").prop("checked", false);

    if (event != null && event.id) {

        editPopupModal = true;
        if (event.tempScheduleRecurrence == "OneTime") {
            $("#rb-one-time-event").prop("checked", true);
            $("#rb-recurrence-event").prop("checked", false);
            hideRecurrenceControls();
        }
        else {
            $("#rb-one-time-event").prop("checked", false);
            $("#rb-recurrence-event").prop("checked", true);
            // show the repeat weeks
            $("#schedule-recurrence-choice").show("fast");
            $("#recurrence-heading").show("fast");

            if (event.isEndDateNull) {
                $("#schedule-end-date").hide("fast");
                $("#no-end-date").show("fast");
                $('#chkbxNoDate').prop('checked', true);
            }
            else {
                $("#schedule-end-date").show("fast");
                $("#no-end-date").show("fast");
                $('#chkbxNoDate').prop('checked', false);
            }

            //$("#FrequencyChoice").val(event.frequencyChoice);
            if (event.frequencyChoice == 1) {
                $("#daily").prop("checked", true);
                $("#txtDailyInterval").val(event.dailyInterval);
            }
            else if (event.frequencyChoice == 2) {
                $("#weekly").prop("checked", true);
                $("#txtWeeklyInterval").val(event.weeklyInterval);
            }
            else if (event.frequencyChoice == 4) {
                $("#monthly").prop("checked", true);
                if (event.isMonthlyDaySelection) {
                    $("#monthlyday").prop("checked", true);
                    $("#txtDayOfMonth").val(event.dayOfMonth);
                    $("#txtMonthlyInterval").val(event.monthlyInterval);
                    resetWeekDays();
                } else {
                    $("#monthlyon").prop("checked", true);
                }
            }
            else if (event.frequencyChoice == 16)
                $("#yearly").prop("checked", true);
            $("#txtAnniversaryDay").val(event.anniversaryDay);
            $('#ddlAnniversaryMonth').val(event.anniversaryMonth);

            OnScheduleReccurenceChoice();

            if (event.frequencyChoice != 1) {

                var array = event.tempDaysOfWeekOptions.split(",");

                $.each(array, function (i) {
                    if (array[i].trim() == "Mon")
                        $('#IsMondaySelected').prop('checked', true);
                    else if (array[i].trim() == "Tue")
                        $('#IsTuesdaySelected').prop('checked', true);
                    else if (array[i].trim() == "Wed")
                        $('#IsWednesdaySelected').prop('checked', true);
                    else if (array[i].trim() == "Thu")
                        $('#IsThursdaySelected').prop('checked', true);
                    else if (array[i].trim() == "Fri")
                        $('#IsFridaySelected').prop('checked', true);
                    else if (array[i].trim() == "Sat")
                        $('#IsSaturdaySelected').prop('checked', true);
                    else if (array[i].trim() == "Sun")
                        $('#IsSundaySelected').prop('checked', true);
                });

                if (event.tempMonthlyIntervalOptions.trim() == "EveryWeek") {
                    $('#IsFirstWeekOfMonthSelected').prop('checked', true);
                    $('#IsSecondWeekOfMonthSelected').prop('checked', true);
                    $('#IsThirdWeekOfMonthSelected').prop('checked', true);
                    $('#IsFourthWeekOfMonthSelected').prop('checked', true);
                    $('#IsLastWeekOfMonthSelected').prop('checked', true);
                }
                else {
                    array = event.tempMonthlyIntervalOptions.split(",");

                    $.each(array, function (i) {
                        if (array[i].trim() == "First")
                            $('#IsFirstWeekOfMonthSelected').prop('checked', true);
                        else if (array[i].trim() == "Second")
                            $('#IsSecondWeekOfMonthSelected').prop('checked', true);
                        else if (array[i].trim() == "Third")
                            $('#IsThirdWeekOfMonthSelected').prop('checked', true);
                        else if (array[i].trim() == "Fourth")
                            $('#IsFourthWeekOfMonthSelected').prop('checked', true);
                        else if (array[i].trim() == "Last")
                            $('#IsLastWeekOfMonthSelected').prop('checked', true);
                    });
                }
            }
        }

        if (!event.anyTimeClockIn) {
            $("#txtClockInStartTime").val(event.clockInStartTime);
            $("#txtClockInEndTime").val(event.clockInEndTime);
            $("#chkbxAnyTimeClockIn").prop("checked", false);
            $('#clockInTiminig').show();
        }
        else {
            $("#chkbxAnyTimeClockIn").prop("checked", true);
            $("#txtClockInStartTime").empty();
            $("#txtClockInEndTime").empty();
            $('#clockInTiminig').hide();
        }

        $('#event-modal input[name="event-start-date"]').datepicker('update', event ? event.originalStartDate : '');
        if ($("#ddlPopupEmployee").length) {
            $('#ddlPopupEmployee').dropdown('set selected', null);
            $('#ddlPopupEmployee').dropdown('set selected', event.employeeId);
        }
        $('#ddlPopupPatient').dropdown('set selected', null);
        $('#ddlPopupPatient').dropdown('set selected', event.referralId);

        if (event.careTypeId != null)
            $("#ddlPopupCareType").val(event.careTypeId);

        if (event.isVirtualVisit) {
            $("#isVirtualVisitYes").prop("checked", true);
            $("#isVirtualVisitNo").prop("checked", false);
        }
        else {
            $("#isVirtualVisitYes").prop("checked", false);
            $("#isVirtualVisitNo").prop("checked", true);
        }

        $("#chkbxIsAnyDay").prop("checked", event.isAnyDay);
        $("#notes").val(event.notes);

        GetPatientPayors(event.referralId, event.payorId);
        GetPatientAuthorizationCodes(event.referralId, event.careTypeId, event.payorId, event.referralBillingAuthorizationId);
    }
    else {
        editPopupModal = false;
        $('#event-modal input[name="event-start-date"]').datepicker('update', event ? event.startDate : '');
        resetAddEventControls();
    }

    $('#event-modal').modal({
        backdrop: 'static',
        keyboard: false
    });
}

function addEvent(event) {
    editPopupModal = false;
    var date = new Date();
    hideRecurrenceControls();
    $('#event-modal input[name="event-start-date"]').datepicker('update', date);
    $('#event-modal input[name="event-end-date"]').datepicker('update', date);
    resetAddEventControls();

}

function resetAddEventControls() {
    if ($("#ddlPopupEmployee").length) {
        $('#ddlPopupEmployee').dropdown('clear');
    }
    $('#ddlPopupPatient').dropdown('clear');
    $("#ddlPopupCareType").val("");
    $("#ddlPatientPayor").empty();
    $("#ddlAuthorizationCode").empty();
    $("#ddlPatientPayor").append('<option value="0">Select</option>');
    $("#ddlAuthorizationCode").append('<option value="0">Select</option>');
    $("#isVirtualVisitYes").prop("checked", false);
    $("#isVirtualVisitNo").prop("checked", true);
    $("#chkbxIsAnyDay").prop("checked", false);
    $("#notes").empty();
    $("#authorizationCodeDetails").hide();
    $("#txtClockInStartTime").val("");
    $("#txtClockInEndTime").val("");
    $('#clockInTiminig').hide();
    $("#authorizationCodeDetails").hide();
    $("#notes").val("");
    $("#chkbxAnyTimeClockIn").prop("checked", true);
    $("#daily").prop("checked", true);
    $("#weekly").prop("checked", false);
    $("#monthly").prop("checked", false);   
    OnScheduleReccurenceChoice();
    $("#txtAnniversaryDay").val("");
    $('#ddlAnniversaryMonth').val(1);
    $('#event-modal').modal({
        backdrop: 'static',
        keyboard: false
    });
}

function deleteEvent(event) {
    //var dataSource = calendar.getDataSource();

    //calendar.setDataSource(dataSource.filter(item => item.id == event.id));

    $.ajax({
        url: '/hc/Schedule/DeleteAppointment',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify({ scheduleID: event.id }),
        success: function (data, status) {
            GetNurseSchedules();
        },
        error: function (html, status, error) {
            console.log('the request is ' + error);
        }
    });
}

function saveEvent() {

    var startDate = $('#event-modal input[name="event-start-date"]').datepicker('getDate');
    var tempEndDate = null;
    var frequencyChoice;
    var anniversaryMonth = $("#ddlAnniversaryMonth").val();

    if ($('#daily').is(':checked')) {
        frequencyChoice = 1;
        resetWeekDays();
        resetMonthWeeks();
    }
    else if ($('#weekly').is(':checked')) {
        frequencyChoice = 2;
        resetMonthWeeks();
    }
    else if ($('#monthly').is(':checked')) {
        frequencyChoice = 4;
        if ($('#monthlyday').is(':checked')) {/* monthly day */
            resetWeekDays();
        }
    }
    else if ($('#yearly').is(':checked')) {
        frequencyChoice = 16;
        resetWeekDays();
        resetMonthWeeks();
    }

    var setEndDate;
    if ($("#chkbxNoDate").is(':checked') || $('#rb-one-time-event').is(':checked'))
        setEndDate = null;
    else {
        setEndDate = $('#event-modal input[name="event-end-date"]').datepicker('getDate');
        tempEndDate = setEndDate;
        setEndDate = setEndDate.toDateString();
    }

    if (tempEndDate != null)
        tempEndDate = WithoutTime(tempEndDate);

    if (authCodeStartDate != null)
        authCodeStartDate = WithoutTime(authCodeStartDate);

    if (authCodeEndDate != null)
        authCodeEndDate = WithoutTime(authCodeEndDate);


    if ($("#ddlPopupEmployee").length && $("#ddlPopupEmployee").val() == null) {
        alert(pleaseSelectEmployee);
        return;
    }
    if ($("#ddlPopupPatient").val() == null) {
        alert(pleaseSelectPatient);
        return;
    }
    if ($("#ddlPopupCareType").val() == "") {
        alert(pleaseSelectCareType);
        return;
    }
    if ($("#rb-recurrence-event").prop('checked') == true &&
        (frequencyChoice == 2 || (frequencyChoice == 4 && $('#monthlyon').is(':checked')))) {

        if ($("#IsSundaySelected").prop('checked') == false && $("#IsMondaySelected").prop('checked') == false &&
            $("#IsTuesdaySelected").prop('checked') == false && $("#IsWednesdaySelected").prop('checked') == false &&
            $("#IsThursdaySelected").prop('checked') == false && $("#IsFridaySelected").prop('checked') == false && $("#IsSaturdaySelected").prop('checked') == false) {
            alert(pleaseSelectADay);
            return;
        }
    }
    if (frequencyChoice == 4 && $('#monthlyday').is(':checked') && isNaN($("#txtDayOfMonth").val())) //monthly day
    {
        alert(errorInvalidDayValue);
        return;
    }
    if (frequencyChoice == 16 && isNaN($("#txtAnniversaryDay").val())) //yearly frequency validation
    {
        alert(errorInvalidDayValue);
        return;
    }
    if (frequencyChoice == 16 && !isValid($("#txtAnniversaryDay").val(), anniversaryMonth - 1)) {  //yearly frequency validation
        alert(errorInvalidMonthDay);
        return;
    }
    if ((authCodeStartDate != null && authCodeEndDate != null && (startDate < authCodeStartDate || startDate > authCodeEndDate))) {
        alert(authorizationCodeInvalidStartDate);
        return;
    }
    if ((authCodeStartDate != null && authCodeEndDate != null && setEndDate != null && (tempEndDate < authCodeStartDate || tempEndDate > authCodeEndDate))) {
        alert(authorizationCodeInvalidEndDate);
        return;
    }
    if ($("#chkbxAnyTimeClockIn").prop('checked') == false && $("#txtClockInStartTime").val() == "") {
        alert(pleaseSelectClockinStartTime);
        return;
    }
    if ($("#chkbxAnyTimeClockIn").prop('checked') == false && $("#txtClockInEndTime").val() == "") {
        alert(pleaseSelectClockinEndTime);
        return;
    }

    var event = {
        id: $('#event-modal input[name="event-index"]').val(),
        startDate: startDate.toDateString(),
        endDate: setEndDate,
        scheduleRecurrence: $('#rb-one-time-event').is(':checked') ? $('#rb-one-time-event').val() : $('#rb-recurrence-event').val(),
        isSundaySelected: $('#IsSundaySelected').is(':checked'),
        isMondaySelected: $('#IsMondaySelected').is(':checked'),
        isTuesdaySelected: $('#IsTuesdaySelected').is(':checked'),
        isWednesdaySelected: $('#IsWednesdaySelected').is(':checked'),
        isThursdaySelected: $('#IsThursdaySelected').is(':checked'),
        isFridaySelected: $('#IsFridaySelected').is(':checked'),
        isSaturdaySelected: $('#IsSaturdaySelected').is(':checked'),
        isFirstWeekOfMonthSelected: $('#monthlyon').is(':checked') ? $('#IsFirstWeekOfMonthSelected').is(':checked') : null,
        isSecondWeekOfMonthSelected: $('#monthlyon').is(':checked') ? $('#IsSecondWeekOfMonthSelected').is(':checked') : null,
        isThirdWeekOfMonthSelected: $('#monthlyon').is(':checked') ? $('#IsThirdWeekOfMonthSelected').is(':checked') : null,
        isFourthWeekOfMonthSelected: $('#monthlyon').is(':checked') ? $('#IsFourthWeekOfMonthSelected').is(':checked') : null,
        isLastWeekOfMonthSelected: $('#monthlyon').is(':checked') ? $('#IsLastWeekOfMonthSelected').is(':checked') : null,
        frequencyChoice: frequencyChoice,
        dailyInterval: $('#daily').is(':checked') ? $("#txtDailyInterval").val() : null,
        weeklyInterval: $('#weekly').is(':checked') ? $("#txtWeeklyInterval").val() : null,
        dayOfMonth: $('#monthlyday').is(':checked') ? $("#txtDayOfMonth").val() : null,
        monthlyInterval: $('#monthlyday').is(':checked') ? $("#txtMonthlyInterval").val() : null,
        isMonthlyDaySelection: $('#monthlyday').is(':checked'),
        careTypeId: $("#ddlPopupCareType").val(),
        employeeId: $("#ddlPopupEmployee").length ? encodeURIComponent($("#ddlPopupEmployee").val()) : '',
        referralId: encodeURIComponent($("#ddlPopupPatient").val()),
        payorId: $("#ddlPatientPayor").val(),
        referralBillingAuthorizationId: $("#ddlAuthorizationCode").val(),
        isVirtualVisit: $('#isVirtualVisitYes').is(':checked') ? true : false,
        isAnyDay: $('#chkbxIsAnyDay').is(':checked'),
        notes: $('#notes').val(),
        anyTimeClockIn: $('#chkbxAnyTimeClockIn').is(':checked'),
        clockInStartTime: $("#txtClockInStartTime").val(),
        clockInEndTime: $("#txtClockInEndTime").val(),
        anniversaryDay: $('#yearly').is(':checked') ? $("#txtAnniversaryDay").val() : null,
        anniversaryMonth: $('#yearly').is(':checked') ? $("#ddlAnniversaryMonth").val() : null,
        nurseScheduleID: $('#event-modal input[name="nurseScheduleId"]').val(),
        scheduleID: $('#event-modal input[name="event-index"]').val()
    };

    $("#loader").show("fast");

    $.ajax({
        url: '/hc/Schedule/SaveAppointment/',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(event),
        success: function (data) {
            $("#loader").hide("fast");
            var message;
            if (!editPopupModal) {
               message = scheduleCreatedMessage;
            }
            else {
                message = scheduleUpdatedMessage;
            }
            if (data.result == -2)
                ShowMessage('No dates matches the criteria to schedule', 'error');
            else if (data.result == -1)
                ShowMessage(errorMessageScheduleConflict, 'error');
            else
                ShowMessage(message);
            if (data.success) {
                GetNurseSchedules();
                $('#event-modal').modal('hide');
            }
        },
        error: function (html, status, error) {
            $("#loader").hide("fast");
            ShowMessage(errorMessageNurseScheduler,'error');
        }
    });    

}

function getDateFormat() {
    return GetOrgDateFormat().toLowerCase();
}

$(function () {

    var currentYear = new Date().getFullYear();

    $('#min-date').datepicker({
        startDate: new Date(currentYear - 1, 0, 1),
        endDate: new Date(currentYear + 1, 11, 31),
        format: getDateFormat()
    });

    $('#event-end-date').datepicker({
        startDate: new Date(currentYear - 1, 0, 1),
        endDate: new Date(currentYear + 1, 11, 31),
        format: getDateFormat()
    });

    $("#loader").hide("fast");
    $("#loaderModal").hide();

    $('#ddlCareTypes')
        .dropdown({
            placeholder: 'All Service',
            fullTextSearch: 'exact'
        });

    $('#ddlEmployees')
        .dropdown({
            placeholder: 'All Employees',
            fullTextSearch: 'exact'
        });

    $('#ddlPatients')
        .dropdown({
            placeholder: 'All Patients',
            fullTextSearch: 'exact'
        });

    $('#ddlPopupEmployee')
        .dropdown({
            placeholder: 'Select',
            fullTextSearch: 'exact'
        });

    $('#ddlPopupPatient')
        .dropdown({
            placeholder: 'Select',
            fullTextSearch: 'exact'
        });

    calendar = new Calendar('#calendar', {
        enableRangeSelection: true,
        enableContextMenu: true,
        style: 'background',
        minDate: new Date(currentYear - 1, 0, 1),
        maxDate: new Date(currentYear + 1, 11, 31),
        contextMenuItems: [
            {
                text: 'Update',
                click: editEvent
            },
            {
                text: 'Delete',
                click: deleteEvent
            }
        ],
        selectRange: function (e) {
            editEvent({ startDate: e.startDate, endDate: e.endDate });
        },
        mouseOnDay: function (e) {
            if (e.events.length > 0) {
                var content = '';

                for (var i in e.events) {
                    if (i != "remove") {
                        content += '<div class="event-tooltip-content">';
                        content += '<div class="row"><div class="col-sm-12" style="color:' + e.events[i].color + '">' + e.events[i].employeeFullName + '</div></div>';
                        content += '<div class="row"><div class="col-sm-12">' + e.events[i].patientFullName + '</div></div>';
                        if (e.events[i].payorId != 0 || e.events[i].payorId != null)
                            content += '<div class="row"><div class="col-sm-12"><b>Payor:</b> ' + (e.events[i].payorName == null ? 'N/A' : e.events[i].payorName) + '</div></div>';
                        content += '<div class="row"><div class="col-sm-12"><b>CareType:</b> ' + e.events[i].careType + '</div></div>';
                        if (e.events.length > 1)
                            content += "<hr/>";
                        content += '</div>';
                    }
                }

                $(e.element).popover({
                    trigger: 'manual',
                    container: 'body',
                    html: true,
                    content: content
                });

                $(e.element).popover('show');
            }
        },
        mouseOutDay: function (e) {
            if (e.events.length > 0) {
                $(e.element).popover('hide');
            }
        },
        dayContextMenu: function (e) {
            $(e.element).popover('hide');
        },
        yearChanged: function (e) {
            selectedYear = e.currentYear;
            if (calendar != null)
                GetNurseSchedules();
        },
        dataSource: []
    });

    GetNurseSchedules();

    $('#save-event').click(function () {
        saveEvent();
    });

    $('#btnAddSchedule').click(function () {
        addEvent();
    });
});

function daysInMonth(m) { // m is 0 indexed: 0-11
    switch (m) {
        case 1:
            return 29;
        case 8: case 3: case 5: case 10:
            return 30;
        default:
            return 31
    }
}

function isValid(d, m) {
    return m >= 0 && m < 12 && d > 0 && d <= daysInMonth(m);
}

function WithoutTime(dateTime) {
    var date = new Date(dateTime.getTime());
    date.setHours(0, 0, 0, 0);
    return date;
}

function GetNurseSchedules() {

    var dataSource = calendar.getDataSource();
    $("#loader").show("fast");

    $.ajax({
        url: '/hc/Schedule/GetNurseSchedules',
        type: 'GET',
        dataType: 'json', // added data type
        data: {
            careTypeIds: $("#ddlCareTypes").val() != null ? encodeURIComponent($("#ddlCareTypes").val()) : "",
            employeeIds: $("#ddlEmployees").val() != null ? encodeURIComponent($("#ddlEmployees").val()) : "",
            referralIds: $("#ddlPatients").val() != null ? encodeURIComponent($("#ddlPatients").val()) : "",
            year: selectedYear
        },
        success: function (res) {
            dataSource = null;
            calendar.setDataSource(dataSource);
            dataSource = calendar.getDataSource();

            for (var i = 0; i < res.length; i++) {

                var event = {
                    id: res[i].ScheduleID,
                    color: res[i].IsVirtualVisit ? "#ff6a00" : "#8FBD7A",
                    name: res[i].PatientFullName,
                    startDate: new Date(parseInt(res[i].StartDate.substr(6))),
                    endDate: res[i].EndDate != null ? new Date(parseInt(res[i].EndDate.substr(6))) : new Date(parseInt(res[i].OriginalEndDate.substr(6))),
                    originalStartDate: new Date(parseInt(res[i].OriginalStartDate.replace("/Date(", "").replace(")/", ""), 10)),
                    originalEndDate: res[i].OriginalEndDate != null ? new Date(parseInt(res[i].OriginalEndDate.replace("/Date(", "").replace(")/", ""), 10)) : null,
                    nurseScheduleID: res[i].NurseScheduleID,
                    scheduleRecurrence: res[i].RecurrencePattern,
                    dayOfMonth: res[i].DayOfMonth,
                    isMonthlyDaySelection: res[i].IsMonthlyDaySelection,
                    dailyInterval: res[i].DailyInterval,
                    weeklyInterval: res[i].WeeklyInterval,
                    monthlyInterval: res[i].MonthlyInterval,
                    isSundaySelected: res[i].IsSundaySelected,
                    isMondaySelected: res[i].IsMondaySelected,
                    isTuesdaySelected: res[i].IsTuesdaySelected,
                    isWednesdaySelected: res[i].IsWednesdaySelected,
                    isThursdaySelected: res[i].IsThursdaySelected,
                    isFridaySelected: res[i].IsFridaySelected,
                    isSaturdaySelected: res[i].IsSaturdaySelected,
                    isFirstWeekOfMonthSelected: res[i].IsFirstWeekOfMonthSelected,
                    isSecondWeekOfMonthSelected: res[i].IsSecondWeekOfMonthSelected,
                    isThirdWeekOfMonthSelected: res[i].IsThirdWeekOfMonthSelected,
                    isFourthWeekOfMonthSelected: res[i].IsFourthWeekOfMonthSelected,
                    isLastWeekOfMonthSelected: res[i].IsLastWeekOfMonthSelected,
                    frequencyChoice: res[i].FrequencyChoice,
                    tempFrequencyTypeOptions: res[i].TempFrequencyTypeOptions,
                    tempScheduleRecurrence: res[i].TempScheduleRecurrence,
                    tempDaysOfWeekOptions: res[i].TempDaysOfWeekOptions,
                    tempMonthlyIntervalOptions: res[i].TempMonthlyIntervalOptions,
                    isEndDateNull: res[i].IsEndDateNull,
                    careTypeId: res[i].CareTypeId,
                    employeeId: res[i].EmployeeId,
                    referralId: res[i].ReferralId,
                    payorId: res[i].PayorId,
                    referralBillingAuthorizationId: res[i].ReferralBillingAuthorizationID,
                    isVirtualVisit: res[i].IsVirtualVisit,
                    careType: res[i].CareType,
                    payorName: res[i].PayorName,
                    employeeFullName: res[i].EmployeeFullName,
                    patientFullName: res[i].PatientFullName,
                    isAnyDay: res[i].IsAnyDay,
                    notes: res[i].Notes,
                    anyTimeClockIn: res[i].AnyTimeClockIn,
                    clockInStartTime: res[i].ClockInStartTime,
                    clockInEndTime: res[i].ClockInEndTime,
                    anniversaryDay: res[i].AnniversaryDay,
                    anniversaryMonth: res[i].AnniversaryMonth
                };

                dataSource.push(event);
            }
            calendar.setDataSource(dataSource);
            $("#loader").hide("fast");
        },
        error: function (html, status, error) {
            $("#loader").hide("fast");
        }
    });

}


$(function () {
    checkRecurrenceType();
});

$("#chkbxNoDate").click(function () {
    if ($("#chkbxNoDate").is(':checked'))
        $("#schedule-end-date").hide("fast");
    else
        $("#schedule-end-date").show("fast");

});

$("#rb-one-time-event").click(function () {
    hideRecurrenceControls();
});

$("#rb-recurrence-event").click(function () {

    // show the repeat weeks
    $("#schedule-recurrence-choice").show("fast");
    $("#recurrence-heading").show("fast");

    // show the end date
    $("#schedule-end-date").show("fast");

    $("#no-end-date").show("fast");

});

$("#daily").click(function () {
    OnScheduleReccurenceChoice();
});
$("#weekly").click(function () {
    OnScheduleReccurenceChoice();
});
$("#monthly").click(function () {
    OnScheduleReccurenceChoice();
});
$("#yearly").click(function () {
    OnScheduleReccurenceChoice();
});

$("#monthlyday").click(function () {
    OnScheduleReccurenceChoice();
});
$("#monthlyon").click(function () {
    OnScheduleReccurenceChoice();
});

function OnScheduleReccurenceChoice() {

    var value = $("#FrequencyChoice").val();

    if ($('#daily').is(':checked')) { /* daily */
        $("#daily-interval-selection").show("fast");
        $("#weekly-interval-selection").hide("fast");
        $("#monthly-interval-selection").hide("fast");
        $("#weekday-selection").hide("fast");
        $("#yearly-interval-selection").hide("fast");
        return;
    }

    if ($('#weekly').is(':checked')) { /* weekly */
        $("#daily-interval-selection").hide("fast");
        $("#weekly-interval-selection").show("fast");
        $("#monthly-interval-selection").hide("fast");
        $("#weekday-selection").show("fast");
        $("#yearly-interval-selection").hide("fast");
        return;
    }

    if ($('#monthly').is(':checked')) {/* monthly */
        $("#daily-interval-selection").hide("fast");
        $("#weekly-interval-selection").hide("fast");
        $("#monthly-interval-selection").show("fast");
        if ($('#monthlyday').is(':checked')) {/* monthly day */
            $("#monthlychoice-day").show("fast");
            $("#monthlychoice-on").hide("fast");
            $("#weekday-selection").hide("fast");
        }
        if ($('#monthlyon').is(':checked')) {/* monthly on */
            $("#monthlychoice-day").hide("fast");
            $("#monthlychoice-on").show("fast");
            $("#weekday-selection").show("fast");
        }
        $("#yearly-interval-selection").hide("fast");

        return;
    }

    if ($('#yearly').is(':checked')) { /* yearly */
        $("#daily-interval-selection").hide("fast");
        $("#weekly-interval-selection").hide("fast");
        $("#monthly-interval-selection").hide("fast");
        $("#weekday-selection").hide("fast");
        $("#yearly-interval-selection").show("fast");
        return;
    }
}

function checkRecurrenceType() {

    if (jQuery("#rb-one-time-event").is(":checked")) {
        hideRecurrenceControls();
    }
}

function hideRecurrenceControls() {
    // hide the repeat weeks
    $("#schedule-recurrence-choice").hide("fast");
    $("#recurrence-heading").hide("fast");

    //// set the frequency value back to empty
    //$("#FrequencyChoice").val("");

    // hide the end date
    $("#schedule-end-date").hide("fast");

    $("#weekly-interval-selection").hide("fast");

    // hide the days of the week
    $("#day-selection").hide("fast");

    $("#no-end-date").hide("fast");

    // hide the monthly interval options
    $("#monthly-interval-selection").hide("fast");

    // hide the yearly selections
    $("#yearly-interval-selection").hide("fast");

    $('#chkbxNoDate').prop('checked', false);

    $("#rb-one-time-event").prop("checked", true);
    $("#rb-recurrence-event").prop("checked", false);

    resetWeekDays();
    resetMonthWeeks();

    $("#daily").prop("checked", true);
    $("#weekly").prop("checked", false);
    $("#monthly").prop("checked", false);
    OnScheduleReccurenceChoice();
}

function resetWeekDays() {
    $("#IsSundaySelected").prop("checked", false);
    $("#IsMondaySelected").prop("checked", false);
    $("#IsTuesdaySelected").prop("checked", false);
    $("#IsWednesdaySelected").prop("checked", false);
    $("#IsThursdaySelected").prop("checked", false);
    $("#IsFridaySelected").prop("checked", false);
    $("#IsSaturdaySelected").prop("checked", false);
}

function resetMonthWeeks() {

    $("#IsFirstWeekOfMonthSelected").prop("checked", false);
    $("#IsSecondWeekOfMonthSelected").prop("checked", false);
    $("#IsThirdWeekOfMonthSelected").prop("checked", false);
    $("#IsFourthWeekOfMonthSelected").prop("checked", false);
    $("#IsLastWeekOfMonthSelected").prop("checked", false);
}

$('#event-modal').on('hidden.bs.modal', function () {

    hideRecurrenceControls();

});

$("#ddlEmployees").change(function () {

    $("#ddlPopupEmployee").val($(this).val());
});

$("#chkbxAnyTimeClockIn").change(function () {

    if ($('#chkbxAnyTimeClockIn').is(':checked') == false)
        $("#clockInTiminig").show("fast");
    else
        $("#clockInTiminig").hide("fast");
});

$("#ddlAuthorizationCode").change(function () {
    setAuthorizationDetails($(this).val());
});

function setAuthorizationDetails(val) {

    var currentYear = new Date().getFullYear();
    var calenderStartDate = new Date(currentYear - 1, 0, 1);
    var calenderEndDate = new Date(currentYear + 1, 11, 31);
    var dateToday = new Date();
    authCodeStartDate = null;
    authCodeEndDate = null;

    for (var index in authCodeData) {
        if (val != 0 && authCodeData[index].ReferralBillingAuthorizationID == val) {
            $("#authStartDate").text(formatDate(authCodeData[index].StartDate));
            $("#authEndDate").text(formatDate(authCodeData[index].EndDate));

            authCodeStartDate = new Date(formatDate(authCodeData[index].StartDate));
            authCodeEndDate = new Date(formatDate(authCodeData[index].EndDate));

            //if (!editPopupModal) {
            //    if (authCodeStartDate >= calenderStartDate && authCodeEndDate <= calenderEndDate) {
            //        $('#event-modal input[name="event-start-date"]').datepicker('update', authCodeStartDate);
            //        $('#event-modal input[name="event-end-date"]').datepicker('update', authCodeEndDate);
            //    }
            //    else {
            //        $('#event-modal input[name="event-start-date"]').datepicker('update', dateToday);
            //        $('#event-modal input[name="event-end-date"]').datepicker('update', dateToday);
            //    }
            //}

            $("#authServiceCode").text(authCodeData[index].ServiceCode);
            $("#authorizationCodeDetails").show("fast");
        }
        else if (val == 0)
            $("#authorizationCodeDetails").hide("fast");
    }
}

$("#ddlPopupPatient").change(function () {
    if ($(this).val() != null) {
        GetPatientPayors($(this).val(), null, null);
        GetPatientAuthorizationCodes($(this).val(), $("#ddlPopupCareType").val(), 0, null);
    }
});

$("#ddlPopupCareType").change(function () {
    if ($("#ddlPopupPatient").val() != null && $(this).val() != null) {
        GetPatientAuthorizationCodes($("#ddlPopupPatient").val(), $(this).val(), 0, null);
    }
});

$("#ddlPatientPayor").change(function () {
    if ($("#ddlPopupPatient").val() != null && $(this).val() != null) {
        GetPatientAuthorizationCodes($("#ddlPopupPatient").val(), $("#ddlPopupCareType").val(), $(this).val(), null);
    }
});

function GetPatientPayors(referralId, payorId) {

    $("#loaderModal").show();

    $.ajax({
        url: '/hc/Schedule/GetPayorsByReferralId',
        type: 'GET',
        data: {
            referralId: encodeURIComponent(referralId)
        },
        dataType: 'json', // added data type
        success: function (data) {
            $("#ddlPatientPayor").empty();
            $("#ddlPatientPayor").append('<option value="0">Select</option>');
            data.forEach(function (data) {
                $("#ddlPatientPayor").append('<option value="' + data.PayorID + '">' + data.PayorName + '</option>');
            });

            if (payorId != null)
                $("#ddlPatientPayor").val(payorId);

            $("#loaderModal").hide();
        },
        error: function (html, status, error) {
            $("#loaderModal").hide();
        }
    });

}

function GetPatientAuthorizationCodes(referralId, careTypeId, payorId, referralBillingAuthorizationId) {
    $("#authorizationCodeDetails").hide("fast");
    $("#loaderModal").show();

    $.ajax({
        url: '/hc/Schedule/GetAuthorizationCodesByReferralId',
        type: 'GET',
        data: {
            referralId: encodeURIComponent(referralId)
        },
        dataType: 'json', // added data type
        success: function (data) {
            $("#ddlAuthorizationCode").empty();
            $("#ddlAuthorizationCode").append('<option value="0">Select</option>');
            payorId = parseInt(payorId)
            data.forEach(function (data) {
                if ((payorId == 0 || data.PayorID == payorId) && 
                    (careTypeId == 0 || data.CareTypeID == careTypeId)) {
                    $("#ddlAuthorizationCode").append('<option value="' + data.ReferralBillingAuthorizationID + '">' + data.AuthorizationCode + '</option>');
                    authCodeData.push(data);
                }
            });

            if (referralBillingAuthorizationId != null) {
                $("#ddlAuthorizationCode").val(referralBillingAuthorizationId);
                setAuthorizationDetails(referralBillingAuthorizationId);
            }

            $("#loaderModal").hide();
        },
        error: function (html, status, error) {
            $("#loaderModal").hide();
        }
    });

}

function formatDate(inputStr) {

    inputStr = new Date(parseInt(inputStr.substr(6)));
    var currentTime = new Date(inputStr);
    var month = currentTime.getMonth() + 1;
    var day = currentTime.getDate();
    var year = currentTime.getFullYear();
    var date = month + "/" + day + "/" + year;
    return date;
}


function SearchFilter() {
    GetNurseSchedules();
}

$('#chkbxNoEndDate').change(function () {
    if (this.checked)
        $('#event-end-date').css('visibility', 'visible');
    else
        $('#event-end-date').css('visibility', 'hidden');
});

/* Set the width of the side navigation to 250px */
function openNav() {
    document.getElementById("mySidenav").style.width = "250px";
    document.getElementById("filter").style.visibility = "hidden";
}

/* Set the width of the side navigation to 0 */
function closeNav() {
    document.getElementById("mySidenav").style.width = "0";
    setTimeout(function () { document.getElementById("filter").style.visibility = "visible"; }, 500);
}