
<div dir="rtl" align="right">

<h1 align="center">المترجم العربي المصغر(Mini Arabic Compiler)</h1>

<p align="center">
  بيئة تعليمية لكتابة البرامج باللغة العربية وتحليلها خطوة بخطوة.
  <br />
  مبني باستخدام C# وWindows Forms، مع محلل أصلي يعتمد على Flex وBison.
</p>

<p align="center">
  <img src="https://img.shields.io/badge/Platform-Windows-2563EB?style=for-the-badge&logo=windows" alt="Windows" />
  <img src="https://img.shields.io/badge/.NET-6.0-7C3AED?style=for-the-badge&logo=dotnet" alt=".NET 6" />
  <img src="https://img.shields.io/badge/Language-Arabic-059669?style=for-the-badge" alt="Arabic" />
  <img src="https://img.shields.io/badge/C%2B%2B-Flex%20%2F%20Bison-D97706?style=for-the-badge&logo=cplusplus" alt="C++ Flex Bison" />
</p>

<p align="center">
  <a href="#المميزات">المميزات</a> ·
  <a href="#التقنيات">التقنيات</a> ·
  <a href="#التشغيل">التشغيل</a> ·
  <a href="#تجربة-اللغة">تجربة اللغة</a> ·
  <a href="#بنية-المشروع">بنية المشروع</a> ·
  <a href="#المساهمة">المساهمة</a>
</p>

</div>

---

## نبذة عن المشروع

**المترجم العربي** هو تطبيق سطح مكتب موجّه للتعلّم وتجربة مفاهيم بناء المترجمات (Compilers). يتيح كتابة تعليمات برمجية بكلمات عربية، ثم فحصها وعرض نتائج مراحل التحليل من خلال واجهة رسومية عربية.

يتكوّن الحل من واجهة **Windows Forms** ومحرك مُدار بلغة **C#**، بالإضافة إلى مكتبة **C++** تستخدم **Flex** للتعرّف على الرموز و**Bison** للتحليل النحوي. ملفات قواعد اللغة والملفات المولّدة موجودة ضمن المشروع.

---

## المميزات

- **تحليل لغوي (Lexical Analysis):** يعرض الرموز المكتشفة ومواقعها في الكود.
- **تحليل نحوي (Syntax Analysis):** فحص قواعد اللغة، مع رسائل للأخطاء وتصحيحات مقترحة.
- **تحليل دلالي (Semantic Analysis):** فحص صحة العلاقات وعرض الأخطاء الناتجة عنه.
- **الشجرة الإعرابية وجدول الرموز:** بناء الشجرة الإعرابية وعرض جدول الرموز.
- **عرض التقرير والنتائج:** تنفيذ مراحل المترجم وعرض التقرير الكامل.
- **محرر وواجهة باللغة العربية:** واجهة كاملة باللغة العربية مع دعم قراءة ملفات البرامج وحفظها.
- **معالجة المرونة في الإدخال:** معالجة بعض الاختلافات الشائعة في الإدخال، مثل الأرقام العربية والفواصل وبعض الكلمات البديلة.

---

## التقنيات

| الجزء | التقنية |
| --- | --- |
| **واجهة المستخدم** | C# وWindows Forms على .NET 6 |
| **التحليل اللغوي** | Flex |
| **التحليل النحوي** | Bison |
| **الربط مع المحلل الأصلي** | C++ DLL وواجهة P/Invoke |
| **المنصة المستهدفة** | Windows، بمعمارية x64 |

---

## التشغيل

### المتطلبات

- نظام تشغيل **Windows**.
- [.NET 6 SDK](https://dotnet.microsoft.com/download/dotnet/6.0) أو إصدار أحدث متوافق مع استهداف `net6.0-windows`.
- لتشغيل المكتبة الأصلية أو إعادة بنائها: **Visual Studio 2022** مع workload **Desktop development with C++** وWindows SDK.

---

### تشغيل التطبيق

من مجلد المشروع، شغّل ملف التشغيل:

```bat
ArabicCompiler.UI\run.bat

```

أو شغّل الواجهة مباشرة باستخدام .NET:

```powershell
dotnet run --project .\ArabicCompiler.UI\ArabicCompiler.UI\ArabicCompiler.UI.csproj

```

ملف `run.bat` يحاول استخدام نسخة DLL الموجودة مسبقًا، ثم يشغّل تطبيق الواجهة. يمكن للواجهة استخدام بعض إمكانات التحليل المُدارة عند عدم توفر المكتبة الأصلية؛ وللاستفادة من محلل Flex/Bison الأصلي، ابنِ المكتبة أولًا.

---

### تشغيل الملف التنفيذي الجاهز

يتضمن المستودع الملف التنفيذي `ArabicCompiler.UI.exe` في مجلده الرئيسي. نزّل المشروع من GitHub أو استنسخه، ثم شغّل الملف التنفيذي. للحصول على عمل المحلل الأصلي، اترك `ArabicCompiler.Native.dll` بجانبه أيضًا. إذا ظهرت رسالة تطلب إطار .NET، ثبّت **.NET 6 Desktop Runtime** لنظام Windows.

للتوزيع على المستخدمين، يمكن نشر `ArabicCompiler.UI.exe` والملفات اللازمة معه ضمن GitHub **Releases**؛ أما ملفات `bin` فهي نواتج بناء محلية وليست بديلًا عن إصدار موثّق.

---

### بناء المكتبة الأصلية (Native DLL)

1. افتح `ArabicCompiler.Native\ArabicCompiler.Native.vcxproj` في Visual Studio.
2. اختر الإعداد `Release` والمنصة `x64`.
3. ابنِ المشروع (Build Solution)، ثم شغّل الواجهة باستخدام ملف التشغيل أعلاه.

---

## تجربة اللغة

يحتوي مجلد `ArabicCompiler.UI\ArabicCompiler.UI\Examples` على البرنامج التجريبي `program.arb`. افتحه من التطبيق أو حمّله في محرر الكود، ثم اختر مرحلة التحليل المطلوبة من القائمة.

تتضمن صياغة اللغة كلمات عربية للبرامج والمتغيرات والأنواع، مثل `برنامج` و`متغير` و`صحيح` و`اطبع`، وتستخدم `؛` لإنهاء التعليمات.

---

## بنية المشروع

```text
compler/
├── ArabicCompiler.Native/
│   ├── Bison/                 # قواعد المحلل النحوي والملفات المولّدة
│   ├── Flex/                  # قواعد المحلل اللغوي والملفات المولّدة
│   └── compiler_api.cpp       # واجهة DLL التي تستدعيها الواجهة
└── ArabicCompiler.UI/
    ├── ArabicCompiler.sln
    └── ArabicCompiler.UI/
        ├── Engine/             # محرك التحليل والتنفيذ والربط مع DLL
        ├── Examples/           # برامج تجريبية باللغة العربية (.arb)
        ├── MainForm.cs         # سلوك واجهة المستخدم
        └── run.bat             # تشغيل التطبيق
