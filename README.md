### WebPageSpeedTest
https://www.webpagetest.org/ üzerinde verilen url leri test edip sonucunu mail atar.

Config dosyasında verilen url leri ard arda test eder. Test sonucu sayfa yüklenme süresini alır, ekran görüntüsü kaydeder. 
Tüm bunları e-posta ile gönderir.

###Kullanım
Testi çalıştırmak için herhangi bir kod değişikliğine gerek yoktur. [aop.config](https://github.com/ibrahimozgon/WebPageSpeedTest/blob/master/WebPageTest/app.config) dosyasını isteğe göre düzenlemek yeterli olacaktır.
```
<appSettings>
<add key="mail.smtp" value="smtp server address" />
<add key="mail.from" value="from mail address" />
<add key="mail.password" value="from mail password" />
<add key="mail.to" value="to mail address" />
<add key="mail.port" value="587" />
<add key="mail.ssl" value="1" />
<add key="blockedDomains" value="adservice.google.com creativecdn.com adservice.google.com.tr securepubads.g.doubleclick.net stats.g.doubleclick.net tpc.googlesyndication.com www.google.com.tr/ads ad.doubleclick.net www.googleadservices.com ajax.googleapis.com www.googleadservices.com ajax.googleapis.com www.googletagmanager.com www.googletagservices.com cdn.segmentify.com googleads.g.doubleclick.net" />
<add key="urls" value="https://www.arabam.com;https://www.arabam.com/ikinci-el/arazi-suv-pick-up" />
<add key="mobileUrls" value="https://www.arabam.com;https://www.arabam.com/ikinci-el/arazi-suv-pick-up" />
</appSettings>
```

**mail. ile baslayan anahtarlar** mail gönderim ayarları içindir.
**blockedDomains** test sırasında bloklanmak istenen domainler içindir.
**urls** web ortamında test edilecek url listesi arada noktalı virgül ile ayrılarak verilmelidir.
**mobileUrls** mobil ortamında test edilecek url listesi arada noktalı virgül ile ayrılarak verilmelidir.


### Çalıştırma
[Nunit Console](http://nunit.org/docs/2.4/nunit-console.html) üzerinden çalıştırılabilir.
