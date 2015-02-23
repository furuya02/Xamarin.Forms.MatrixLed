using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

using Xamarin.Forms;

namespace MatrixLed {
    public class App : Application {
        public App() {
            MainPage = new MyPage();
        }

        protected override void OnStart() {
            // Handle when your app starts
        }

        protected override void OnSleep() {
            // Handle when your app sleeps
        }

        protected override void OnResume() {
            // Handle when your app resumes
        }
    }

    internal class MyPage : ContentPage {
        public MyPage() {

            var ar = new List<Led>();

            BackgroundImage = "back.png";//背景画像

            var absoluteLayout = new AbsoluteLayout();

            //送信文字の表示用ラベル
            var label = new Label{
                Text="0x0000000000000000",
                FontFamily = "HelveticaNeue-Thin",
				FontSize = 30,
                TextColor = Color.White,
                BackgroundColor = Color.FromRgba(0,0,0,70),
                WidthRequest = 375,//iPhone6
                HeightRequest = 100,
                XAlign = TextAlignment.Center,
                YAlign = TextAlignment.Center

            };
            absoluteLayout.Children.Add(label, new Point(0, 667-90));

            //タッチイベントの処理
            var gr = new TapGestureRecognizer(); 
            gr.Tapped += (s, e) => {
                var led = (Led) s;
                
                //ON/OFFの反転
                led.Sw = !led.Sw;

                //LEDのON/OFFデータをを文字列として取得する
                var dataStr = GetDataStr(ar);
                
                //ラベルへの表示
                label.Text = "0x" + dataStr;

                //WebAPIのコール
                Api(dataStr);

            };

            //LEDの生成と表示
            const int top = 100;
            const int left = 3;
            const int width = 46;
            for (var y = 0; y < 8; y++)
            {
                for (var x = 0; x < 8; x++) {
                    var led = new Led();
                    ar.Add(led);
                    led.GestureRecognizers.Add(gr); //タッチイベントの検知を追加
                    absoluteLayout.Children.Add(led, new Point(left + x*width, top + y*width));
                }
            }

            Content = absoluteLayout;
        }
        //WebAPIのコール
        async void Api(String daataStr){
            var httpClient = new HttpClient();
            const string deviceId = "53ff6xxxxxxxxxxxx02567";//デバイスID
            const string accessToken = "50179xxxxxxxxxxxxxxxxxxxxxxxx95413e";//アクセストークン
            const string funcName = "func";//WebAPIのファンクション名
            var data = new FormUrlEncodedContent(new Dictionary<string, string>{
                    {"access_token", accessToken},
                    {"params", daataStr}
                });
            //URLはデバイスIDとファンクション名で決まる
            var url = string.Format("https://api.spark.io/v1/devices/{0}/{1}", deviceId, funcName);
            await httpClient.PostAsync(url, data);
        }

        //LEDのON/OFFデータをを文字列として取得する
        private string GetDataStr(List<Led> ar) {
            var sb = new StringBuilder();

            for (var r = 0; r < 8; r++) {
                var dat = 0;
                for (var c = 0; c < 8; c++) {
                    var index = r*8 + c;
                    if (ar[index].Sw) {
                        dat |= 1 << (7 - c);
                    }
                }
                sb.Append(Convert.ToString(dat, 16));
            }
            return sb.ToString();
        }


    }


}
