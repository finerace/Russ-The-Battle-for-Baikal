mergeInto(LibraryManager.library, {
	
	OpenAuthDialog: function(){
		ysdk.auth.openAuthDialog().then(() => {
			
			player = null;
			initPlayer().then(_player => {
				player = _player;

				MyGameInstance.SendMessage("SaveSystem","LoadDataAUTH");
				MyGameInstance.SendMessage("Yandex","CloseAuthMenu");
				console.log('Player InAuth YES');

			}).catch(err => {
        		console.log('Player InAuth init ERROR');
        		console.log(err);
    		});

		}).catch(() => {
			console.log('Player InAuth ERROR');
                });
	},

	CheckPlayerAuth: function(){
		if (player.getMode() === 'lite')
            {
                MyGameInstance.SendMessage("Yandex","OpenAuthMenu");
            }
	},

	ShowAdv : function () {
		MyGameInstance.SendMessage("Yandex", "AudioOff");
		ysdk.adv.showFullscreenAdv({
			callbacks: {
				onClose: function(wasShown) {
					MyGameInstance.SendMessage("Yandex", "AudioOn");
				},
				onError: function(error) {
					MyGameInstance.SendMessage("Yandex", "AudioOn");
				}
			}
		})
	},

	ShowAdvRevive : function () {

		MyGameInstance.SendMessage("Yandex", "AudioOff");

		var reward = false;

		ysdk.adv.showRewardedVideo({
			callbacks: {
				onOpen: () => {
					console.log('Video ad open.');
					MyGameInstance.SendMessage("Yandex", "AudioOff");
				},
				onRewarded: () => {
					console.log('Rewarded!');
					reward = true;
				},
				onClose: () => {
					console.log('Video ad closed.');
					MyGameInstance.SendMessage("Yandex", "AudioOn");

					if(reward == true) {
						MyGameInstance.SendMessage("Yandex", "RevivePlayer");
					}

				}, 
				onError: (e) => {
					console.log('Error while open video ad:', e);
					MyGameInstance.SendMessage("Yandex", "AudioOn");
				}
			}
		})
	},

	ShowAdvDouble : function () {

		MyGameInstance.SendMessage("Yandex", "AudioOff");

		ysdk.adv.showRewardedVideo({
			callbacks: {
				onOpen: () => {
					console.log('Video ad open.');
					MyGameInstance.SendMessage("Yandex", "AudioOff");
				},
				onRewarded: () => {
					console.log('Rewarded!');
					MyGameInstance.SendMessage("Yandex", "DoubleCoins");
				},
				onClose: () => {
					console.log('Video ad closed.');
					MyGameInstance.SendMessage("Yandex", "AudioOn");
				}, 
				onError: (e) => {
					console.log('Error while open video ad:', e);
					MyGameInstance.SendMessage("Yandex", "AudioOn");
				}
			}
		})
	},

	aSaveData : function(data) {

		console.log(data);

		try {
			localStorage.setItem(UTF8ToString("saveKey"), UTF8ToString(data));
		}
		catch (e) {
			console.error('Save to Local Storage error: ', e.message);
		}
	},

	aLoadData : function() {
		var returnStr = localStorage.getItem(UTF8ToString("saveKey"));
		
		if(returnStr == null)
		{
			return null;
		}

		var bufferSize = lengthBytesUTF8(returnStr) + 1;
		var buffer = _malloc(bufferSize);
		stringToUTF8(returnStr, buffer, bufferSize);

		console.log(buffer);

		return buffer;
	},

});