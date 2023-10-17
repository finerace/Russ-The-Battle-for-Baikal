mergeInto(LibraryManager.library, {
	
	OpenAuthDialog: function(){
		ysdk.auth.openAuthDialog().then(() => {
			ysdk.getPlayer({scopes: false}).then(_player => {
				player = _player;
			}).catch(err => {
      		// Ошибка при инициализации объекта Player.
  			});
			aLoadData();
		});
	},

	CheckPlayerAuth: function(){
		if (player.getMode() === 'lite')
            {
                player.SendMessage("Yandex","OpenAuthMenu");
            }
	},

	ShowAdv : function () {
		MyGameInstance.SendMessage("Yandex", "AudioOff");
		MyGameInstance.SendMessage("Yandex", "MenuBack");		

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

	aSaveData: function(data) {
		var dataString = UTF8ToString(data);
		var myobj = JSON.parse(dataString);
		if(player.getMode() !== "lite")
		{
			player.setData(myobj);
			console.log(dataString);
		}
	},

	aLoadData: function() {
		if(player.getMode() !== "lite")
		{
			player.getData().then(_data => {
				const myJSON = JSON.stringify(_data);
				MyGameInstance.SendMessage("SaveSystem", "LoadDataa", myJSON);
				console.log(myJSON);
			})
		};
	},

});