package 
{
	import com.bit101.components.Label;
	import com.bit101.components.PushButton;
	import com.bit101.components.Style;
	import flash.display.Sprite;
	import flash.events.Event;
	import flash.events.TimerEvent;
	import flash.system.Capabilities;
	import flash.utils.Timer;
	
	/**
	 * ...
	 * @author umhr
	 */
	public class Main extends Sprite 
	{
		private var _timer:Timer;
		
		public function Main():void 
		{
			if (stage) init();
			else addEventListener(Event.ADDED_TO_STAGE, init);
		}
		
		private function init(e:Event = null):void 
		{
			removeEventListener(Event.ADDED_TO_STAGE, init);
			// entry point
			_timer = new Timer(50, 200);
			_timer.addEventListener(TimerEvent.TIMER, timer_timer);
			
			var text:String = "FlashPlayer" + Capabilities.version;
			if (Capabilities.isDebugger) {
				text += "デバッグプレイヤーです。";
			}else {
				text += "デバッグプレイヤーではありません。";
			}
			Style.embedFonts = false;
			Style.fontName = "PF Ronda Seven";
			Style.fontSize = 12;
			
			new Label(this, 16, 16, text);
			
			var obj:Object = { };
			//obj.hoge.foo;
			
			new PushButton(this, 16, 60, "trace(Plase PrintScreen!)", onPush).width = 200;
			new PushButton(this, 16, 120, "Timer Start!", onSetTimer);
		}
		
		private function onSetTimer(e:Event):void {
			_timer.reset();
			_timer.start();
		}
		private function onPush(e:Event):void {
			trace("Please PrintScreen!");
		}
		
		private function timer_timer(e:TimerEvent):void 
		{
			if (Math.random() > 0.95) {
				onPush(null);
			}else {
				trace("BooFooWoo" + Math.random());
			}
			
		}
		
	}
	
}