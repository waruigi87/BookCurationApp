window.getApiConfig = () => {
    // 環境変数やAmplify設定から動的に取得
    return {
        apiBaseUrl: window.location.hostname.includes('localhost')
            ? 'https://v3wbyayobb.execute-api.ap-northeast-1.amazonaws.com/default/api/v1'
            : 'https://v3wbyayobb.execute-api.ap-northeast-1.amazonaws.com/default/api/v1'
    };
};
