$(document).ready(function () {

    GetDashcamDevices(null);
    InitializeDropdown();
    function InitializeDropdown() {
        $('#ddlVehicles').select2({
            placeholder: "Select Vehicle",
            allowClear: true
        });
    }
    function GetDashcamDevices(deviceImei) {
        $.ajax({
            url: '/GetDashcamDeviceList',
            type: 'POST',
            data: { deviceImei: deviceImei },
            success: function (response) {

                $("#ddlVehicles").empty();
                $("#ddlVehicles").append(` <option value="">Select Vehicle</option>`);
                response.forEach((item) => {
                    $("#ddlVehicles").append(`<option value="${item.deviceImei}" channels="${item.channels}"> ${item.vehicleRegNo}</option>`);
                });
            }

        });
    };

    const $ddlChannels = $("#ddlChannels");
    const $ddlVehicles = $("#ddlVehicles");
    const $txtFromTime = $("#txtFromTime");
    const $txtToTime = $("#txtToTime");
    const $btnSearch = $("#btnSearch");
    const $mandatoryFields = $(".mand");
    const emptyPlayerState = document.getElementById('emptyPlayerState');
    const loadingPlayerState = document.getElementById('loadingPlayerState');
    const historyStreaming = document.getElementById('history_streaming');



    const now = new Date();
    const hours = String(now.getHours()).padStart(2, '0');
    const minutes = String(now.getMinutes()).padStart(2, '0');
    const formattedTime = `${hours}:${minutes}`;
    $txtToTime.val(formattedTime);
    $txtFromTime.val('00:00');


    $ddlChannels.attr("disabled", true);

    let deviceImei = '';
    $btnSearch.on("click", function () {
     
       


        let i = 0;
        deviceImei = $ddlVehicles.val();
        const channel = $ddlChannels.val();
        const fromTime = $txtFromTime.val();
        const toTime = $txtToTime.val();
      

        $mandatoryFields.each(function () {
            const $this = $(this);
            if ($this.val() === "" || $this.val() === null) {
                $this.css('border-color', 'red');
                i = 1;
            } else {
                $this.css('border-color', '');
            }
        });


        if (i === 0) {
            $.ajax({
                url: '/SendHistoryCommand',
                type: 'POST',
                data: { deviceImei, fromTime, toTime, channel },
                beforeSend: function () {
                    $btnSearch.text('Please Wait...');
                },
                complete: function () {
                    $btnSearch.text('Search');
                },
                success: function (response) {

                    if (response == "request timeout") {
                        showNotification(response, "danger");
                        return;
                    }
                    else if (response == "time error") {
                        showNotification("unable to fetch history more than 40 minutes", "danger");
                        return;
                    }
                    else if (response == "The device is offline or timed out, and the command is converted to an offline command") {
                        showNotification(response, "danger");
                        return;
                    }
                    else if (response == "Device not online") {
                        showNotification(response, "danger");
                        return;
                    }

                    let videoElement = document.getElementById('history_streaming');
                    let flvPlayer = flvjs.createPlayer({
                        type: 'flv',
                        url: `http://172.105.59.200:8881/${channel}/${deviceImei}.history.flv`,
                    });
                    flvPlayer.attachMediaElement(videoElement);
                    flvPlayer.load();
                    flvPlayer.play();
                    loadingPlayerState.style.display = 'none';
                    emptyPlayerState.style.display = 'none';
                    historyStreaming.style.display = 'block';

                    //setTimeout(function () {
                    //    uploadHistoryVideos(deviceImei);
                    //}, 3000);
                },
                error: function (err) {

                }
            });
        }
        else {
            showNotification("all fields are mandatory", "danger");
        }
    });
    $(document).on("click", "#btnReset", function () {
        $("#ddlVehicles").val("").trigger("change");
        $("#ddlChannels").empty();
        $("#txtFromTime").val("");
        $("#txtToTime").val("");

    });


    $mandatoryFields.change(function () {
        const $this = $(this);
        if ($this.val() === "" || $this.val() === null) {
            $this.css('border-color', 'red');
            i = 1;
        } else {
            $this.css('border-color', '');
        }
    });


    $ddlVehicles.on("change", function () {


        const totalChannels = $(this).find('option:selected').attr("channels");
        $ddlChannels.empty();

        if (totalChannels && totalChannels !== "") {

            let options = "";
            for (let i = 1; i <= totalChannels; i++) {
                options += `<option value="${i}">C${i}</option>`;
            }
            $ddlChannels.append(options).attr("disabled", false);
        } else {
            $ddlChannels.attr("disabled", true);
        }
    });


    // Theme Toggle
    const themeToggle = document.getElementById('themeToggle');
    const icon = themeToggle.querySelector('i');

    themeToggle.addEventListener('click', function () {
        document.body.classList.toggle('dark-theme');

        if (document.body.classList.contains('dark-theme')) {
            icon.classList.remove('fa-moon');
            icon.classList.add('fa-sun');
            localStorage.setItem('theme', 'dark');
        } else {
            icon.classList.remove('fa-sun');
            icon.classList.add('fa-moon');
            localStorage.setItem('theme', 'light');
        }
    });

    // Check for saved theme preference
    const savedTheme = localStorage.getItem('theme');
    if (savedTheme === 'dark') {
        document.body.classList.add('dark-theme');
        icon.classList.remove('fa-moon');
        icon.classList.add('fa-sun');
    }

    function showNotification(message, type) {
        $('.alert.position-fixed').remove();

        const notification = $(`
                    <div class="alert alert-${type} position-fixed top-0 end-0 m-3" style="z-index: 2000">
                        <i class="fas fa-${type === 'success' ? 'check' : 'exclamation'}-circle me-2"></i> ${message}
                    </div>
                `);

        $('body').append(notification);

        setTimeout(() => {
            notification.remove();
        }, 4000);
    }

});