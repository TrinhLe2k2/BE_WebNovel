

        // pagination
        (function (name) {
            var container = $('#pagination-' + name);
            if (!container.length) return;
            var sources = function () {
                var result = [];

                for (var i = 1; i < 2002; i++) {
                    result.push(i);
                }

                return result;
            }();

            var options = {
                dataSource: sources,
                pageSize: 50,
                showSizeChanger: false,
                showGoInput: true,
                showGoButton: true,
                autoHidePrevious: true,
                autoHideNext: true,
                callback: function (response, pagination) {
                    window.console && console.log(response, pagination);

                    var dataHtml = '<div class="row g-3">';

                    $.each(response, function (index, item) {
                        dataHtml += 
                        '<div class="col-6 col-sm-4 col-md-3">' +
                            '<a href="./DocTruyen.html"><div class="p-2 Box-Chapter-Link"><span class="Chapter-Link">' +
                                'Chương ' + item + ': Dù bỏ mình ma tâm vẫn không hối hận' +
                            '</span></div></a>' +
                        '</div>';
                    });

                    dataHtml += '</div>';

                    container.prev().html(dataHtml);
                }
            };

            //$.pagination(container, options);

            container.addHook('beforeInit', function () {
                window.console && console.log('beforeInit...');
            });
            container.pagination(options);

            container.addHook('beforePageOnClick', function () {
                window.console && console.log('beforePageOnClick...');
                //return false
            });
        })('paginationForChapter');


        //js cmt
        // Dữ liệu bình luận mẫu (thay thế cho việc gửi yêu cầu tới máy chủ)
        const comments = [
        {id: 1, name: "bình luận 1" },
        {id: 2, name: "bình luận 2" },
        {id: 3, name: "bình luận 3" },
        {id: 4, name: "bình luận 4" },
        {id: 5, name: "bình luận 5" },
        {id: 6, name: "bình luận 6" },
        {id: 7, name: "bình luận 7" },
        {id: 8, name: "bình luận 8" },
        {id: 9, name: "bình luận 9" },
        {id: 10, name: "bình luận 10" },
        {id: 11, name: "bình luận 11" },
        {id: 12, name: "bình luận 12" },
        {id: 13, name: "bình luận 13" },
        {id: 14, name: "bình luận 14" },
        // Các bình luận tiếp theo ...
        ];

        const commentsChild = [
        {id: 1, commentID: 1, content: "Comment child 1" },
        {id: 2, commentID: 2, content: "Comment child 2" },
        {id: 3, commentID: 3, content: "Comment child 3" },
        {id: 4, commentID: 4, content: "Comment child 4" },
        {id: 5, commentID: 5, content: "Comment child 5" },
        {id: 6, commentID: 6, content: "Comment child 6" },
        {id: 7, commentID: 7, content: "Comment child 7" },
        {id: 8, commentID: 8, content: "Comment child 8" },
        {id: 9, commentID: 9, content: "Comment child 9" },
        {id: 10, commentID: 10, content: "Comment child 10" },
        {id: 11, commentID: 11, content: "Comment child 11" },
        {id: 12, commentID: 12, content: "Comment child 12" },
        {id: 13, commentID: 13, content: "Comment child 13" },
        {id: 14, commentID: 14, content: "Comment child 14" }
        ];

        let page = 1;
        const commentsPerPage = 5; // Số bình luận hiển thị trên mỗi lần tải thêm

        function displayComments(comments) {
            const commentList = $("#commentList");
            comments.forEach((comment) => {
                var commentElement = `<li class="list-group-item"> <div class="Comment"> <div class="Comment-Parent"> <div class="User mt-3 d-flex align-items-center"> <img class="Avatar rounded-circle" src="./Content/assets/img/truyen1.jpg" alt="Avatar" style="width: 40px; height: 40px; object-fit: cover" /> <h5 class="mb-0 ms-2">User Name</h5> <div class="ms-auto position-relative" id="Tab-Action"> <input type="checkbox" hidden id="Cmt_ellipsis-vertical${comment.id}" /> <label id="Cmt_ellipsis-verticalLabel${comment.id}" for="Cmt_ellipsis-vertical${comment.id}" style="cursor: pointer" ><i class="fa-solid fa-ellipsis-vertical p-2"></i ></label> <ul class="list-group position-absolute d-none flex-column align-items-start" id="ActionListCmt${comment.id}" style="width: 500%; border: solid 1px #333; background-color: #fff" > <li class="list-group-item p-0 w-100"> <a href="#" class="Item-Link p-2 d-block w-100 text-start" >Chỉnh sửa</a > </li> <li class="list-group-item p-0 w-100"> <a href="#" class="Item-Link p-2 d-block w-100 text-start">Xóa</a> </li> </ul> </div> </div> <p class="Comment-content my-3"> Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed vel lorem at massa facilisis malesuada. </p> <div class="Action d-flex"> <ul class="nav"> <li class="nav-item"> <a href="#" class="nav-link"> <i class="fa-solid fa-book"></i> Chương 23 </a> </li> <li class="nav-item"> <a class="nav-link Nav-Like${comment.id}"> <input type="checkbox" hidden id="CheckLike${comment.id}" /> <label for="CheckLike${comment.id}" style="cursor: pointer"> <i class="fa-solid fa-thumbs-up p-1"></i> 999M </label> </a> </li> <li class="nav-item"> <a class="nav-link Nav-Reply${comment.id}"> <input type="checkbox" hidden id="CheckReply${comment.id}" /> <label for="CheckReply${comment.id}" style="cursor: pointer"> <i class="fa-solid fa-reply p-1"></i> 1 Trả lời </label> </a> </li> <li class="nav-item"> <a href="#" class="nav-link disabled"> <i class="fa-solid fa-clock"></i> 1 phút trước </a> </li> </ul> </div> </div> <div class="Comment-Child${comment.id} Comment-Child ms-5 d-none"> <ul class="list-group list-group-flush"> <li class="list-group-item"> <div class="User mt-3 d-flex align-items-center"> <img class="Avatar rounded-circle" src="./Content/assets/img/truyen1.jpg" alt="Avatar" style="width: 40px; height: 40px; object-fit: cover" /> <h5 class="mb-0 ms-2">User Name</h5> <div class="ms-auto position-relative" id="Tab-Action"> <input type="checkbox" hidden id="CmtChild_ellipsis-vertical${comment.id}" /> <label id="CmtChild_ellipsis-verticalLabel${comment.id}" for="CmtChild_ellipsis-vertical${comment.id}" style="cursor: pointer" ><i class="fa-solid fa-ellipsis-vertical p-2"></i ></label> <ul class="list-group position-absolute d-none flex-column align-items-start" id="ActionListCmtChild${comment.id}" style=" width: 500%; border: solid 1px #333; background-color: #fff; " > <li class="list-group-item p-0 w-100"> <a href="#" class="Item-Link p-2 d-block w-100 text-start" >Chỉnh sửa</a > </li> <li class="list-group-item p-0 w-100"> <a href="#" class="Item-Link p-2 d-block w-100 text-start" >Xóa</a > </li> </ul> </div> </div> <p class="Comment-content my-3"> Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed vel lorem at massa facilisis malesuada. </p> <div class="Action d-flex"> <ul class="nav"> <li class="nav-item"> <a class="p-2 nav-link Nav-Like${comment.id}"> <input type="checkbox" hidden id="CheckLikeCmtChild${comment.id}" /> <label for="CheckLikeCmtChild${comment.id}" style="cursor: pointer"> <i class="fa-solid fa-thumbs-up"></i> 999M </label> </a> </li> <li class="nav-item"> <a href="#" class="p-2 nav-link disabled"> <i class="fa-solid fa-clock"></i> 1 phút trước </a> </li> </ul> </div> </li> </ul> <form class="d-flex flex-column my-3"> <textarea class="form-control" id="comment" rows="2" placeholder="Nhập bình luận của bạn" ></textarea> <button type="button" class="btn btn-primary my-2 align-self-end"> Trả Lời </button> </form> </div> </div> </li>`
        commentList.append(commentElement);

        // js like cmt parent
        var clike = document.querySelector('#Main .ActionNovel .tab-content .Comment-List .Comment .Comment-Parent .Action #CheckLike' + comment.id);
        var likeLink = document.querySelector('#Main .ActionNovel .tab-content .Comment-List .Comment .Comment-Parent .Action .Nav-Like' + comment.id);
        clike.addEventListener('change', function () {
                    if (this.checked) {
            likeLink.classList.add('active');
                    } else {
            likeLink.classList.remove('active');
                    }
                });

        // js like cmt child
        var clikeCmtChild = document.querySelector('#Main .ActionNovel .tab-content .Comment-List .Comment .Comment-Child'+ comment.id + ' .Action #CheckLikeCmtChild' + comment.id);
        var likeLinkCmtChild = document.querySelector('#Main .ActionNovel .tab-content .Comment-List .Comment .Comment-Child'+ comment.id + ' .Action .Nav-Like' + comment.id);
        clikeCmtChild.addEventListener('change', function () {
                    if (this.checked) {
            likeLinkCmtChild.classList.add('active');
                    } else {
            likeLinkCmtChild.classList.remove('active');
                    }
                });

        // js cmt child
        var crep = document.querySelector('#Main .ActionNovel .tab-content .Comment-List .Comment .Comment-Parent .Action #CheckReply' + comment.id);
        var child = document.querySelector('#Main .ActionNovel .tab-content .Comment-List .Comment .Comment-Child' + comment.id);
        var navReply = document.querySelector('#Main .ActionNovel .tab-content .Comment-List .Comment .Comment-Parent .Action .Nav-Reply' + comment.id);
        crep.addEventListener('change', function () {
                    if (this.checked) {
            navReply.classList.add('active');
        child.classList.remove('d-none');
                    } else {
            navReply.classList.remove('active');
        child.classList.add('d-none');
                    }
                });
            });


            // js chỉnh sửa, xóa cho comment parent
            comments.forEach(element => {
                var ellipsisVerticalChk = document.getElementById('Cmt_ellipsis-vertical' + element.id);
        var actionList = document.getElementById('ActionListCmt' + element.id);
        ellipsisVerticalChk.addEventListener('change', function () {
                    if (this.checked) {
            actionList.classList.remove('d-none');
                    } else {
            actionList.classList.add('d-none');
                    }
                });
            });

            // js chỉnh sửa, xóa cho comment child
            comments.forEach(element => {
                var ellipsisVerticalChkCmtChild = document.getElementById('CmtChild_ellipsis-vertical' + element.id);
        var actionList = document.getElementById('ActionListCmtChild' + element.id);
        ellipsisVerticalChkCmtChild.addEventListener('change', function () {
                    if (this.checked) {
            actionList.classList.remove('d-none');
                    } else {
            actionList.classList.add('d-none');
                    }
                });
            });



        // js ẩn drop block
        document.addEventListener('click', function(event) {
            // cmt parent
            comments.forEach(element => {
                var ellipsisVerticalChkParent = document.getElementById('Cmt_ellipsis-vertical' + element.id);
                var ellipsisVerticalChkParentLabel = document.getElementById('Cmt_ellipsis-verticalLabel' + element.id);
                var ActionListCmtParent = document.getElementById('ActionListCmt' + element.id);
                var editBtnCmtParent = ActionListCmtParent.querySelector('.Item-Link');

                if (!ActionListCmtParent.contains(event.target) && !ellipsisVerticalChkParent.contains(event.target) && !ellipsisVerticalChkParentLabel.contains(event.target)) {
                    ellipsisVerticalChkParent.checked = false;
                    ActionListCmtParent.classList.add('d-none');
                }

                editBtnCmtParent.addEventListener('click', function () {
                    ellipsisVerticalChkParent.checked = false;
                    ActionListCmtParent.classList.add('d-none');
                });
            });

                // cmt child
                commentsChild.forEach(element => {
                    var ellipsisVerticalChkChild = document.getElementById('CmtChild_ellipsis-vertical'+ element.commentID);
        var ellipsisVerticalChkChildLabel = document.getElementById('CmtChild_ellipsis-verticalLabel'+ element.commentID);
        var ActionListCmtChild = document.getElementById('ActionListCmtChild'+ element.commentID);
        var editBtnCmtChild = ActionListCmtChild.querySelector('.Item-Link');

        if(!ActionListCmtChild.contains(event.target) && !ellipsisVerticalChkChild.contains(event.target) && !ellipsisVerticalChkChildLabel.contains(event.target)) {
            ellipsisVerticalChkChild.checked = false;
        ActionListCmtChild.classList.add('d-none');
                    }

        editBtnCmtChild.addEventListener('click', function() {
            ellipsisVerticalChkChild.checked = false;
        ActionListCmtChild.classList.add('d-none');
                    });
                });
            });
        }

        function loadMoreComments() {
            $("#loadMoreBtn").hide(); // Ẩn nút tải thêm

        // Hiển thị spinner trong 1 giây trước khi tải dữ liệu
        $("#loadingSpinner").removeClass("d-none");
            setTimeout(() => {
            $("#loadingSpinner").addClass("d-none"); // Ẩn spinner sau 1 giây
        $("#loadMoreBtn").show(); // Hiển thị lại nút tải thêm

        // Sử dụng slice để lấy số bình luận cần hiển thị trong lần tải thêm
        const startIndex = (page - 1) * commentsPerPage;
        const endIndex = startIndex + commentsPerPage;
        const commentsToShow = comments.slice(startIndex, endIndex);

                if (commentsToShow.length > 0) {
            displayComments(commentsToShow);

        page++; // Tăng số trang lên để chuẩn bị cho lần tải thêm tiếp theo
                }

                // Ẩn nút tải thêm và hiển thị thông báo khi hết bình luận
                if (startIndex + commentsToShow.length >= comments.length) {
            $("#loadMoreBtn").hide();
        $("#commentList").append("<p class='text-muted'>Không còn bình luận nào khác</p>");
                }
            }, 500);
        }

        $(document).ready(function () {
            // Xử lý sự kiện khi nhấn nút tải thêm
            $("#loadMoreBtn").on("click", function () {
                loadMoreComments();
            });

        // Hiển thị bình luận ban đầu khi trang tải
        loadMoreComments();
            
        });