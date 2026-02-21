$(document).ready(function () {

    let activeStreams = [];
    var flvPlayer = {};

    GetDashcamDevices(null);
    $(document).on("click", ".dash_cam_1", function () {
        let status = $(this).attr("status");
        if (status.toLowerCase() == "offline") {
            showNotification("Command not sent as the device is offline. Please check for updates and refresh the page.", "danger");
            $(this).prop('checked', false);
            return;
        }
        let channel = 1;
        let deviceImei = $(this).attr("imei");
        let deviceModel = $(this).attr("model");
        let liveStreamingUrl = '';
        let videoElement = null;


        if ($(this).is(':checked')) {
            $(this).prop('checked', true);
            if (deviceImei !== "") {
                $.ajax({
                    url: '/SendDashcamInstruction',
                    type: 'POST',
                    data: { DeviceImei: deviceImei },   
                    success: function (response) {
                        if (response.message == "") {
                            showNotification("Device not online", "danger");
                            return;
                        }

                        if (deviceModel === "JC371") {
                            for (let channel = 1; channel <= 3; channel++) {
                                setTimeout(() => {
                                    let assignedStream = null;
                                    for (let i = 1; i <= 6; i++) {
                                        if (!$('#liveStream_' + i).attr("data-busy")) {
                                            assignedStream = i;
                                            break;
                                        }
                                    }

                                    if (assignedStream) {
                                        let liveStreamingUrl = `http://172.105.59.200:8881/${channel}/${deviceImei}.flv`;
                                        let videoElement = document.getElementById('liveStream_' + assignedStream);

                                        $('#liveStream_' + assignedStream)
                                            .attr("data-busy", "true")
                                            .attr("data-deviceimei", deviceImei)
                                            .attr("data-channel", channel);

                                        flvPlayer[assignedStream] = flvjs.createPlayer({
                                            type: 'flv',
                                            url: liveStreamingUrl
                                        });

                                        flvPlayer[assignedStream].attachMediaElement(videoElement);
                                        flvPlayer[assignedStream].load();
                                        flvPlayer[assignedStream].play();
                                        $('.channel_' + deviceImei).prop('checked', true);
                                        showLiveChannels(assignedStream, deviceImei, channel);

                                    } else {
                                        showNotification("All streams are currently in use!","danger");

                                    }

                                }, (channel - 1) * 3000);
                            }

                        }
                        else {
                            let assignedStream = null;

                            for (let i = 1; i <= 6; i++) {
                                if (!$('#liveStream_' + i).attr("data-busy")) {
                                    assignedStream = i;
                                    break;
                                }
                            }

                            if (assignedStream) {
                                let liveStreamingUrl = `http://172.105.59.200:8881/live/0/${deviceImei}.flv`;
                                let videoElement = document.getElementById('liveStream_' + assignedStream);

                                $('#liveStream_' + assignedStream)
                                    .attr("data-busy", "true")
                                    .attr("data-deviceimei", deviceImei)
                                    .attr("data-channel", "1");

                                flvPlayer[assignedStream] = flvjs.createPlayer({
                                    type: 'flv',
                                    url: liveStreamingUrl
                                });

                                flvPlayer[assignedStream].attachMediaElement(videoElement);
                                flvPlayer[assignedStream].load();
                                flvPlayer[assignedStream].play();
                               
                                showLiveChannels(assignedStream, deviceImei, "1");

                            } else {
                              
                                showNotification("All streams are currently in use!", "danger");
                            }
                        }

                    },
                });
            }
        } else {
            for (var i = 1; i <= 6; i++) {

                if (deviceModel == "JC371") {
                    liveStreamingUrl = `http://172.105.59.200:8881/${channel}/${deviceImei}.flv`
                } else {
                    liveStreamingUrl = `http://172.105.59.200:8881/live/${channel-1}/${deviceImei}.flv`;
                }
                let imei = $('#liveStream_' + i).data("deviceimei");
                let ch = $('#liveStream_' + i).data("channel");
                if (imei == deviceImei && ch == "1") {

                    videoElement = document.getElementById('liveStream_' + i);
                    flvPlayer[i] = flvjs.createPlayer({
                        type: 'flv',
                        url: liveStreamingUrl
                    });

                    flvPlayer[i].attachMediaElement(videoElement);
                    flvPlayer[i].detachMediaElement();

                    $('#liveStream_' + i)
                        .removeAttr("data-busy")
                        .removeAttr("data-deviceimei")
                        .removeAttr("data-channel");

                    hideChannel(i);
                    break;
                }
            }
        }
    });

    $(document).on("click", ".dash_cam_2", function () {
        let status = $(this).attr("status");
        if (status.toLowerCase() == "offline") {
            showNotification("Command not sent as the device is offline. Please check for updates and refresh the page.", "danger");
            $(this).prop('checked', false);
            return;
        }
        let channel = 2;
        let deviceImei = $(this).attr("imei");
        let deviceModel = $(this).attr("model");
        let liveStreamingUrl = '';
        let videoElement = null;


        if ($(this).is(':checked')) {
            $(this).prop('checked', true);
            if (deviceImei !== "") {
                $.ajax({
                    url: '/SendDashcamInstruction',
                    type: 'POST',
                    data: { DeviceImei: deviceImei },
                    success: function (response) {


                        if (deviceModel === "JC371") {
                            for (let channel = 1; channel <= 3; channel++) {
                                setTimeout(() => {
                                    let assignedStream = null;
                                    for (let i = 1; i <= 6; i++) {
                                        if (!$('#liveStream_' + i).attr("data-busy")) {
                                            assignedStream = i;
                                            break;
                                        }
                                    }

                                    if (assignedStream) {
                                        let liveStreamingUrl = `http://172.105.59.200:8881/${channel}/${deviceImei}.flv`;
                                        let videoElement = document.getElementById('liveStream_' + assignedStream);

                                        $('#liveStream_' + assignedStream)
                                            .attr("data-busy", "true")
                                            .attr("data-deviceimei", deviceImei)
                                            .attr("data-channel", channel);

                                        flvPlayer[assignedStream] = flvjs.createPlayer({
                                            type: 'flv',
                                            url: liveStreamingUrl
                                        });

                                        flvPlayer[assignedStream].attachMediaElement(videoElement);
                                        flvPlayer[assignedStream].load();
                                        flvPlayer[assignedStream].play();
                                        $('.channel_' + deviceImei).prop('checked', true);
                                        showLiveChannels(assignedStream, deviceImei, "2");

                                    } else {
                                        showNotification("All streams are currently in use!", "danger");
                                    }

                                }, (channel - 1) * 3000);
                            }

                        }
                        else {
                            let assignedStream = null;

                            for (let i = 1; i <= 6; i++) {
                                if (!$('#liveStream_' + i).attr("data-busy")) {
                                    assignedStream = i;
                                    break;
                                }
                            }

                            if (assignedStream) {
                                let liveStreamingUrl = `http://172.105.59.200:8881/live/1/${deviceImei}.flv`;
                                let videoElement = document.getElementById('liveStream_' + assignedStream);

                                $('#liveStream_' + assignedStream)
                                    .attr("data-busy", "true")
                                    .attr("data-deviceimei", deviceImei)
                                    .attr("data-channel", 2);

                                flvPlayer[assignedStream] = flvjs.createPlayer({
                                    type: 'flv',
                                    url: liveStreamingUrl
                                });

                                flvPlayer[assignedStream].attachMediaElement(videoElement);
                                flvPlayer[assignedStream].load();
                                flvPlayer[assignedStream].play();

                                showLiveChannels(assignedStream, deviceImei, "2");

                            } else {
                                showNotification("All streams are currently in use!", "danger");
                            }
                        }

                    },
                });
            }
        } else {
            for (var i = 1; i <= 6; i++) {

                if (deviceModel == "JC371") {
                    liveStreamingUrl = `http://172.105.59.200:8881/${channel}/${deviceImei}.flv`
                } else {
                    liveStreamingUrl = `http://172.105.59.200:8881/live/${channel-1}/${deviceImei}.flv`;
                }
                let imei = $('#liveStream_' + i).data("deviceimei");
                let ch = $('#liveStream_' + i).data("channel");
                if (imei == deviceImei && ch == "2") {

                    videoElement = document.getElementById('liveStream_' + i);
                    flvPlayer[i] = flvjs.createPlayer({
                        type: 'flv',
                        url: liveStreamingUrl
                    });

                    flvPlayer[i].attachMediaElement(videoElement);
                    flvPlayer[i].detachMediaElement();

                    $('#liveStream_' + i)
                        .removeAttr("data-busy")
                        .removeAttr("data-deviceimei")
                        .removeAttr("data-channel");

                    hideChannel(i);
                    break;
                }
            }
        }
    });

    $(document).on("click", ".dash_cam_3", function () {
        let status = $(this).attr("status");
        if (status.toLowerCase() == "offline") {
            showNotification("Command not sent as the device is offline. Please check for updates and refresh the page.", "danger");
            $(this).prop('checked', false);
            return;
        }
        let channel = 3;
        let deviceImei = $(this).attr("imei");
        let deviceModel = $(this).attr("model");
        let liveStreamingUrl = '';
        let videoElement = null;


        if ($(this).is(':checked')) {
            $(this).prop('checked', true);
            if (deviceImei !== "") {
                $.ajax({
                    url: '/SendDashcamInstruction',
                    type: 'POST',
                    data: { DeviceImei: deviceImei },
                    success: function (response) {


                        if (deviceModel === "JC371") {
                            for (let channel = 1; channel <= 3; channel++) {
                                setTimeout(() => {
                                    let assignedStream = null;
                                    for (let i = 1; i <= 6; i++) {
                                        if (!$('#liveStream_' + i).attr("data-busy")) {
                                            assignedStream = i;
                                            break;
                                        }
                                    }

                                    if (assignedStream) {
                                        let liveStreamingUrl = `http://172.105.59.200:8881/${channel}/${deviceImei}.flv`;
                                        let videoElement = document.getElementById('liveStream_' + assignedStream);

                                        $('#liveStream_' + assignedStream)
                                            .attr("data-busy", "true")
                                            .attr("data-deviceimei", deviceImei)
                                            .attr("data-channel", channel);

                                        flvPlayer[assignedStream] = flvjs.createPlayer({
                                            type: 'flv',
                                            url: liveStreamingUrl
                                        });

                                        flvPlayer[assignedStream].attachMediaElement(videoElement);
                                        flvPlayer[assignedStream].load();
                                        flvPlayer[assignedStream].play();
                                        $('.channel_' + deviceImei).prop('checked', true);
                                        showLiveChannels(assignedStream, deviceImei, "3");

                                    } else {
                                        showNotification("All streams are currently in use!", "danger");
                                    }

                                }, (channel - 1) * 3000);
                            }

                        }
                        else {
                            let assignedStream = null;

                            for (let i = 1; i <= 6; i++) {
                                if (!$('#liveStream_' + i).attr("data-busy")) {
                                    assignedStream = i;
                                    break;
                                }
                            }

                            if (assignedStream) {
                                let liveStreamingUrl = `http://172.105.59.200:8881/live/2/${deviceImei}.flv`;
                                let videoElement = document.getElementById('liveStream_' + assignedStream);

                                $('#liveStream_' + assignedStream)
                                    .attr("data-busy", "true")
                                    .attr("data-deviceimei", deviceImei)
                                    .attr("data-channel", "3");

                                flvPlayer[assignedStream] = flvjs.createPlayer({
                                    type: 'flv',
                                    url: liveStreamingUrl
                                });

                                flvPlayer[assignedStream].attachMediaElement(videoElement);
                                flvPlayer[assignedStream].load();
                                flvPlayer[assignedStream].play();

                                showLiveChannels(assignedStream, deviceImei, "3");

                            } else {
                                showNotification("All streams are currently in use!", "danger");
                            }
                        }

                    },
                });
            }
        } else {
            for (var i = 1; i <= 6; i++) {

                if (deviceModel == "JC371") {
                    liveStreamingUrl = `http://172.105.59.200:8881/${channel}/${deviceImei}.flv`
                } else {
                    liveStreamingUrl = `http://172.105.59.200:8881/live/${channel-1}/${deviceImei}.flv`;
                }
                let imei = $('#liveStream_' + i).data("deviceimei");
                let ch = $('#liveStream_' + i).data("channel");
                if (imei == deviceImei && ch == "3") {

                    videoElement = document.getElementById('liveStream_' + i);
                    flvPlayer[i] = flvjs.createPlayer({
                        type: 'flv',
                        url: liveStreamingUrl
                    });

                    flvPlayer[i].attachMediaElement(videoElement);
                    flvPlayer[i].detachMediaElement();

                    $('#liveStream_' + i)
                        .removeAttr("data-busy")
                        .removeAttr("data-deviceimei")
                        .removeAttr("data-channel");

                    hideChannel(i);
                    break;
                }
            }
        }
    });
    function showLiveChannels(assignedStream, deviceImei,channel) {
        let video = $('#liveStream_' + assignedStream);
        video.closest('.video-content').find('.video-placeholder').hide();
        video.closest('.video-content').prev('.video-header').find('.channel_number').text(`CH${channel} for : ${deviceImei}`);
        video.show();
    }
    function hideChannel(channel) {
        let video = $('#liveStream_' + channel);
        video.closest('.video-content').find('.video-placeholder').show();
        video.closest('.video-content').prev('.video-header').find('.channel_number').text(`Channel ${channel}`);
        video.hide();
    }
    function GetDashcamDevices(deviceImei) {
        $.ajax({
            url: '/GetDashcamDeviceList',
            type: 'POST',
            data: { deviceImei: deviceImei },
            success: function (response) {

                $("#vehiclesList").empty();

                response.forEach((item) => {
                    let statusClass = "";
                    let channelHtml = "";

                    for (let i = 1; i <= item.channels; i++) {
                        channelHtml += `
                <div class="channel-item">
                    <input type="checkbox"
                        class="channel-checkbox dash_cam_${i}  channel_${item.deviceImei}"
                        model="${item.model}"
                        imei="${item.deviceImei}"
                        status="${item.status}"
                        channel="${i}">
                    <label class="channel-label"
                        for="channel-${item.deviceImei}-${i}">
                        CH${i}
                    </label>
                </div>
            `;
                    }

                    let vehicleCard = `
            <div class="vehicle-card">
                <div class="vehicle-header">
                    <div class="vehicle-info">
                        <div class="vehicle-status status-${item.status.toLowerCase() == "offline" ? "offline" : "online"}"></div>
                        <span class="vehicle-name">${item.vehicleRegNo}</span>
                    </div>
                    <div class="vehicle-actions">
                        <button class="action-btn"
                            data-action="image"
                            data-device="${item.deviceImei}" data-totalchannels="${item.channels}">
                            <i class="fas fa-camera"></i>
                        </button>
                        <button class="action-btn"
                            data-action="video"
                            data-device="${item.deviceImei}" data-totalchannels="${item.channels}"> 
                            <i class="fas fa-video"></i>
                        </button>
                    </div>
                </div>

                <div class="vehicle-channels">
                   <div style="display:flex;    justify-content: space-between;">
                    ${channelHtml}
                   </div>

                     <div class="vehicle-meta">
                    Last Update: ${item.lastUpdate}
                </div>
                </div>

               
            </div>
        `;

                    $("#vehiclesList").append(vehicleCard);
                });
            }

        });
    };

    $(document).on("click", ".vehicle-header", function () {

        this.parentElement.classList.toggle('expanded');
    });
    $(document).on("keyup", "#searchInput", function () {

        let searchValue = $(this).val();
        GetDashcamDevices(searchValue);
    });
    $(document).on("click", '[data-action="image"]', function () {
        let deviceImei = $(this).data('device');
        $("#txtHiddenDeviceImei").val(deviceImei);
        let totalChannel = $(this).data('totalchannels');
        let channelHtml = "";
        $(".imei_number").text(deviceImei);
        $('#imageModal').find(".channel-select").empty();
       
        for (var i = 1; i <= 2; i++) {
            channelHtml += `
                    <div class="channel-option">
                        <input type="checkbox" id="image-channel-${i}" class="channel-checkbox channel_${i}_image">
                        <label for="image-channel-${i}">CH${i}</label>
                    </div>
            `;
        }
        $('#imageModal').find(".channel-select").html(channelHtml);
        $('#imageModal')
            .attr('data-device', deviceImei)
            .addClass('active');
    });

    $(document).on("click", "#captureImage", function () {
        let deviceImei = $("#txtHiddenDeviceImei").val();
        let channel_1 = false;
        let channel_2 = false;
        if ($("input.channel_1_image:checked").length > 0) {
            channel_1 = true;
        }
        if ($("input.channel_2_image:checked").length > 0) {
            channel_2 = true;
        }
        if (channel_1 || channel_2) {
            $.ajax({
                type: "POST",
                url: "CaptureImage",
                data: { deviceImei, channel_1, channel_2 },
                beforeSend: function () {
                    $("#captureImage").text('Wait..');
                },
                success: function (response) {
                    if (response == "Device not online") {
                        showNotification(response, "danger");
                        return;
                    }
                    showNotification(response, "success");
                    setTimeout(()=> {
                        window.location.reload(true);
                    },3000)
                },
                complete: function () {
                    $("#btnCaptureImage").text('Capture');
                }
            });
        }
        else {
            showNotification("please select minimum 1 channel", "danger");
        }

    });

    $(document).on("click", '[data-action="video"]', function () {
        let deviceImei = $(this).data('device');
        $("#txtHiddenDeviceImei").val(deviceImei);
        let totalChannel = $(this).data('totalchannels');
        let channelHtml = "";
        $(".imei_number").text(deviceImei);
        $('#imageModal').find(".channel-select").empty();
        $(".imei_number").text(deviceImei);
        $('#videoModal').find(".channel-select").empty();
        for (var i = 1; i <= 2; i++) {
            channelHtml += `
                    <div class="channel-option">
                        <input type="checkbox" id="video-channel-${i}" class="channel-checkbox channel_${i}_video">
                        <label for="video-channel-${i}">CH${i}</label>
                    </div>
            `;
        }
        $('#videoModal').find(".channel-select").html(channelHtml);
        $('#videoModal')
            .attr('data-device', deviceImei)
            .addClass('active');
    });
    $(document).on("click", "#captureVideo", function () {
        let deviceImei = $("#txtHiddenDeviceImei").val();
        let seconds = $("#ddlSeconds").val();
        let channel;
        if ($("input.channel_1_video:checked").length > 0) {
            channel = 1;
        }
        if ($("input.channel_2_video:checked").length > 0) {
            channel = 2;
        }
        if ($("input.channel_1_videochecked").length > 0 && $("input.channel_2_video:checked").length > 0) {
            showNotification("You can select only one channel at a time", "danger");
            return;
        }
        if (channel == undefined) {
            showNotification("Please select atlease 1 channel", "danger");
            return;
        }
        if (seconds) {
            $.ajax({
                type: "POST",
                url: "CapturedVideo",
                data: { deviceImei, seconds, channel },
                beforeSend: function () {
                    $("#captureVideo").text('Wait..');
                },
                success: function (response) {
                    if (response == "Device not online") {
                        showNotification(response, "danger");
                        return;
                    }
                    showNotification(response, "success");
                    setTimeout(() => {
                        window.location.reload(true);
                    }, 3000);
                },
                complete: function () {
                    $("#captureVideo").text('Capture');
                }
            });
        }
        else {
            showNotification("Please select seconds","danger");
        }

    });

    // Modal Handling
    const closeButtons = document.querySelectorAll('.modal-close, .btn-secondary');


    closeButtons.forEach(button => {
        button.addEventListener('click', function () {
            imageModal.classList.remove('active');
            videoModal.classList.remove('active');
        });
    });


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


    // Layout Controls - FIXED
    const layoutButtons = document.querySelectorAll('.layout-btn');
    const videoGrid = document.getElementById('videoGrid');

    // Create 6 video slots
    for (let i = 1; i <= 6; i++) {
        const videoItem = document.createElement('div');
        videoItem.className = 'video-item';
        videoItem.innerHTML = `
                    <div class="video-header">
                        <div class="video-title">
                            <i class="fas fa-video"></i>
                            <span class="channel_number">Channel ${i}</span>
                        </div>
                        <div class="video-actions">
                            <button class="action-btn fullscreen-btn">
                                <i class="fas fa-expand"></i>
                            </button>
                        </div>
                    </div>
                    <div class="video-content">
                        <div class="video-placeholder">
                            <i class="fas fa-satellite-dish placeholder-icon"></i>
                            <div class="no-video-text">No device selected for stream </div>
                        </div>
                        <video class="video-player" id="liveStream_${i}" style="display: none;" controls></video>
                    </div>
                `;
        videoGrid.appendChild(videoItem);
    }

    // Fixed layout switching functionality
    layoutButtons.forEach(button => {
        button.addEventListener('click', function () {
            // Remove active class from all buttons
            layoutButtons.forEach(btn => btn.classList.remove('active'));

            // Add active class to clicked button
            this.classList.add('active');

            // Get the layout value
            const layout = this.getAttribute('data-layout');

            // Reset grid classes
            videoGrid.className = 'video-grid';

            // Apply the selected layout
            switch (layout) {
                case '1':
                    videoGrid.classList.add('grid-1');
                    break;
                case '2':
                    videoGrid.classList.add('grid-2');
                    break;
                case '4':
                    videoGrid.classList.add('grid-4');
                    break;
                case '6':
                    videoGrid.classList.add('grid-6');
                    break;
            }
        });
    });

    // Fullscreen functionality
    document.querySelectorAll('.fullscreen-btn').forEach(button => {
        button.addEventListener('click', function () {
            const videoItem = this.closest('.video-item');
            videoItem.classList.toggle('fullscreen');

            const icon = this.querySelector('i');
            if (videoItem.classList.contains('fullscreen')) {
                icon.classList.remove('fa-expand');
                icon.classList.add('fa-compress');
            } else {
                icon.classList.remove('fa-compress');
                icon.classList.add('fa-expand');
            }
        });
    });




   
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
