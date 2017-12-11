# WebPageSpeedTest
https://www.webpagetest.org/ üzerinde verilen url leri test edip sonucunu mail atar.

Config dosyasında verilen url leri ard arda test eder. Test sonucu sayfa yüklenme süresini alır, ekran görüntüsü kaydeder. 
Tüm bunları e-posta ile gönderir.

<add key="mail.smtp" value="smtp server address" />
<add key="mail.from" value="from mail address" />
<add key="mail.password" value="from mail password" />
<add key="mail.to" value="to mail address" />
<add key="mail.port" value="587" />
<add key="mail.ssl" value="1" />
<add key="urls" value="https://www.arabam.com;https://www.arabam.com/ikinci-el/arazi-suv-pick-up" />
noktalı virgül ile ayrılan urlleri ard arda test eder.

Nunit console üzerinden çalıştırılabilir.
http://nunit.org/docs/2.4/nunit-console.html
