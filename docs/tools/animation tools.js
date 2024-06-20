


function stop_till_key(root) {
	root.stop()
	
	window.onkeydown = (key_event) => {
		if (key_event.code == "Space") {
			root.play();
		}
	}
}

