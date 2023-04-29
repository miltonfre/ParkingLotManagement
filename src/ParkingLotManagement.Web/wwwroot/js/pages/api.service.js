const ApiService = () => {

    const Get = (url, success, error) => {
        $.ajax({
            type: "GET",
            url: url,
            success: success,
            error: error
        });
    }

    const Post = (url, data,success, error) => {
        $.ajax({
            type: "POST",
            url: url,
            data: data,
            success: success,
            error: error
        });
    }
}

export { ApiService }