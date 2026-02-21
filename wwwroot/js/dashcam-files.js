$(document).ready(function () {
    let allVideoUrl = [];
    let allImageUrl = [];
    GetDashcamDevices(null);
    InitializeDropdown();
    function InitializeDropdown() {
        $('#ddlVehicles').select2({
            placeholder: "Select Vehicle",
            allowClear: true,
            width: '100%'
        });
        $('#ddlVehiclesHistory').select2({
            placeholder: "Select Vehicle",
            allowClear: true,
            width: '100%'
        });
        
    }
    function GetDashcamDevices(deviceImei) {
        $.ajax({
            url: '/GetDashcamDeviceList',
            type: 'POST',
            data: { deviceImei: deviceImei },
            success: function (response) {

                $("#ddlVehicles").empty();
                $("#ddlVehiclesHistory").empty();
                $("#ddlVehiclesHistory").append(` <option value="">Select Vehicle</option>`);
                $("#ddlVehicles").append(` <option value="">Select Vehicle</option>`);
                $("#ddlVehiclesHistory").append(` <option value="">Select Vehicle</option>`);
                response.forEach((item) => {
                    $("#ddlVehicles").append(`<option value="${item.deviceImei}" channels="${item.channels}"> ${item.vehicleRegNo}</option>`);
                    $("#ddlVehiclesHistory").append(`<option value="${item.deviceImei}" channels="${item.channels}"> ${item.vehicleRegNo}</option>`);
                });

            }

        });
    };
    $(document).on("click", "#btnSearch", function () {
        GetFiles();
    });

    function GetFiles() {
        var i = 0;
        var Day = $("#ddlDay").val();
        var DeviceImei = $("#ddlVehicles").val();
        var FromDate = '';
        var ToDate = '';
        if (Day == "Custom") {
            FromDate = $("#txtFromDate").val();
            ToDate = $("#txtToDate").val();
        }
        $(".mandatory").each(function () {
            if ($(this).val() == "" || $(this).val() == null) {
                $(this).css('border-color', 'red');
                i = 1;
            }
            else {
                $(this).css('border-color', '');
            }
        });
        if (i == 0) {
            $("#btnSearch").html('<i class="fas fa-spinner fa-spin me-1"></i>Searching...').attr("disabled", true);
            $.ajax({
                url: '/GetDashcamFiles',
                type: 'POST',
                data: { DeviceImei: DeviceImei, Day: Day, FromDate: FromDate, ToDate: ToDate },
                success: function (res) {
                    $("#btnSearch").html('<i class="fas fa-search"></i> Search').attr("disabled", false);
                    if (res.length > 0) {
                       
                        $("#videosGallery").empty();
                        $("#photosGallery").empty();
                        playlist = [];
                        index = 0;
                        for (let i = 0; i < res.length; i++) {
                            let fileType = res[i].fileType.toLowerCase();
                            if (fileType == "picture") {
                                $("#photosGallery").append(`<div class="file-card">
                                    <div class="file-thumbnail show_image">
                                        <div class="file-type">JPG</div>
                                           <img src="${res[i].fileUrl}" alt="Image File" style="width:100%; height:100%; object-fit:cover;">
                                    </div>
                                    <div class="file-info">
                                       <div class="file-name" style="display:flex; justify-content:space-between; align-items:center;">
                                            <span>${res[i].alertName}</span>
                                            <a href="${res[i].fileUrl}" download>
                                                <i class="fas fa-download" style="cursor:pointer;"></i>
                                            </a>
                                        </div>
                                        <div class="file-meta">
                                            <span>${res[i].date}</span>
                                        </div>
                                    </div>
                                    </div>`);
                                allImageUrl.push(res[i].fileUrl);
                                $("#btnDownloadAllPhotos").show();

                            }
                            if (fileType.toLowerCase() == "video") {
                                $("#videosGallery").append(`
                                    <div class="file-card">
                                       <div class="file-thumbnail show_video">
                                           <div class="file-type">MP4</div>
                                            <video width="100%" height="100%" controls>
        <source src="${res[i].fileUrl}" type="video/mp4">
        Your browser does not support the video tag.
    </video>
                                       </div>
                                       <div class="file-info">
                                          <div class="file-name" style="display:flex; justify-content:space-between; align-items:center;">
                                            <span>${res[i].alertName}</span>
                                            <a href="${res[i].fileUrl}" download>
                                                <i class="fas fa-download" style="cursor:pointer;"></i>
                                            </a>
                                        </div>
                                           <div class="file-meta">
                                               <span>${res[i].date}</span>
                                           </div>
                                       </div>
                                       </div>`);

                                allVideoUrl.push(res[i].fileUrl);

                                if (res[i].fileUrl.includes(".mp4")) {
                                    playlist.push(res[i].fileUrl);
                                }
                                $("#btnDownloadAllVideos").show();
                                $("#btnPlayAllVideos").show();
                                
                            }
                            

                        }
                    }
                    else {
                        showNotification("record not found!!","danger");
                    }
                },
            });
        }
        else {
            showNotification("all fields are mandatory","danger");
        }
    }


    let $player = document.getElementById("player");
    let index = 0;
    let playlist = []; 

    const status = document.getElementById("status");

    function playNext() {
        if (index < playlist.length) {
            status.textContent = `Playing video ${index + 1} of ${playlist.length}`;
            $player.src = playlist[index++];
            $player.play();
        } else {
            status.textContent = "All videos completed";
        }
    }

    $player.addEventListener("ended", playNext);

    $(document).on("click", "#btnPlayAllVideos", function () {

        if (playlist.length === 0) {
            showNotification("No videos to play!","danger");
            return;
        }

        index = 0;

        $("#emptyVideoState").hide();
        $("#loadingVideoState").show();

        setTimeout(function () {
            $("#loadingVideoState").hide();
            $("#player").show();
            playNext();
        }, 1000);

    });

  
   
    $(document).on("change", "#ddlDay", function () {
        if ($(this).val() == "Custom") {
            $(".DateDiv").show();
            $("#txtFromDate").addClass("mandatory");
            $("#txtToDate").addClass("mandatory");
        }
        else {
            $(".DateDiv").hide();
            $("#txtFromDate").removeClass("mandatory");
            $("#txtToDate").removeClass("mandatory");
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

    // Tab functionality
    const tabButtons = document.querySelectorAll('.tab-btn');
    const tabPanes = document.querySelectorAll('.tab-pane');

    tabButtons.forEach(button => {
        button.addEventListener('click', function () {
            const tabId = this.getAttribute('data-tab');

            // Update active tab button
            tabButtons.forEach(btn => btn.classList.remove('active'));
            this.classList.add('active');

            // Show active tab pane
            tabPanes.forEach(pane => pane.classList.remove('active'));
            document.getElementById(`${tabId}-tab`).classList.add('active');
        });
    });
    $(document).on("click", "#btnDownloadAllPhotos", function () {

        if (allImageUrl.length === 0) {
            showNotification("No videos found!", "danger");
            return;
        }

        allImageUrl.forEach(function (url, index) {

            setTimeout(function () {

                let link = document.createElement("a");
                link.href = url;
                link.download = "image_" + (index + 1) + ".jpg";
                document.body.appendChild(link);
                link.click();
                document.body.removeChild(link);

            }, index * 1000); // 1 second gap between each

        });

    });

   
  

 
  

    $(document).on("click", "#btnDownloadAllVideos", function () {

        if (allVideoUrl.length === 0) {
            showNotification("No videos found!","danger");
            return;
        }

        allVideoUrl.forEach(function (url, index) {

            setTimeout(function () {

                let link = document.createElement("a");
                link.href = url;
                link.download = "video_" + (index + 1) + ".mp4";
                document.body.appendChild(link);
                link.click();
                document.body.removeChild(link);

            }, index * 1000); // 1 second gap between each

        });

    });


    const imageViewer = document.getElementById('imageViewer');
    const fullImage = document.getElementById('fullImage');
    const modalClose = document.querySelector('.modal-close');
    
   
    modalClose.addEventListener('click', function () {
        imageViewer.classList.remove('active');
    });

    imageViewer.addEventListener('click', function (e) {
        if (e.target === imageViewer) {
            imageViewer.classList.remove('active');
        }
    });

    $(document).on("click", ".show_image", function (e) {
        let url = $(this).find('img').attr("src");
        fullImage.src = url;
        imageViewer.classList.add('active');
    });


  

    const $txtDateHistory = $("#txtDateHistory");
    const $txtFromTimeHistory = $("#txtFromTimeHistory");
    const $txtToTimeHistory = $("#txtToTimeHistory");
    const $btnGetHistoryVideos = $("#btnGetHistoryVideos");
    const $ddlVehiclesHistory = $("#ddlVehiclesHistory");
    const $ddlChannelsHistory = $("#ddlChannelsHistory");
    const $mandatoryFields = $(".mandatory_history");

    const now = new Date();
    const hours = String(now.getHours()).padStart(2, '0');
    const minutes = String(now.getMinutes()).padStart(2, '0');
    const formattedTime = `${hours}:${minutes}`;
    $txtToTimeHistory.val(formattedTime);
    $txtFromTimeHistory.val('00:00');

    let deviceImei = "";
    $btnGetHistoryVideos.on("click", function () {
        let i = 0;
        deviceImei = $ddlVehiclesHistory.val();
        const date = $txtDateHistory.val();
        const channel = $ddlChannelsHistory.val();
        const fromTime = $txtFromTimeHistory.val();
        const toTime = $txtToTimeHistory.val();
        let d = new Date();
        d.setDate(d.getDate() - 6);
        let weekAgoDate = d.toISOString().split('T')[0];
        if (date < weekAgoDate) {
            $("#btnGetHistoryVideos").html('<i class="fas fa-history"></i> Get History Videos');
            showNotification("You can only view data from the last 7 days. Please choose a date within the past week.", "danger");
          
            return;
        }

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
            $("#btnGetHistoryVideos").html('<i class="fas fa-spinner fa-spin me-1"></i>Searching...');
            $.ajax({
                url: '/GetDashcamHistoryVideo',
                type: 'POST',
                data: { deviceImei, date, fromTime, toTime, channel },
                success: function (response) {
                    if (response == "Device not online") {
                        $("#btnGetHistoryVideos").html('<i class="fas fa-history"></i> Get History Videos');
                        showNotification("Device not online", "danger");
                        return;
                    }
                    setTimeout(function () {
                        uploadHistoryVideos(deviceImei);
                    }, 5000);
                },
                error: function (err) {

                }
            });
        }
    });

    function uploadHistoryVideos(deviceImei) {
        $.ajax({
            url: '/UploadDashcamHistoryVideos',
            type: 'POST',
            data: {},
            success: function (response) {
                if (response) {
                    showNotification(response,"danger")
                }
                $("#btnGetHistoryVideos").html('<i class="fas fa-history"></i> Get History Videos');
            },
            error: function (err) {

            }
        });
    }
    $ddlVehiclesHistory.on("change", function () {


        const totalChannels = $(this).find('option:selected').attr("channels");
        $ddlChannelsHistory.empty();

        if (totalChannels && totalChannels !== "") {

            let options = "";
            for (let i = 1; i <= totalChannels; i++) {
                options += `<option value="${i}">C${i}</option>`;
            }
            $ddlChannelsHistory.append(options).attr("disabled", false);
        } else {
            $ddlChannelsHistory.attr("disabled", true);
        }
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


