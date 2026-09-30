#include "pch.h"
#include <stddef.h>

#ifdef CONST
#undef CONST
#endif

#ifdef TRUE
#undef TRUE
#endif

#ifdef FALSE
#undef FALSE
#endif

extern "C"
{
#include "Bison/parser.tab.h"
}

/*
    واجهة الربط المشتركة مع Flex و Bison.
*/
extern "C"
{
    int yyparse(void);
    int yylex(void);

    typedef struct yy_buffer_state* YY_BUFFER_STATE;
    YY_BUFFER_STATE yy_scan_string(const char* text);
    void yy_delete_buffer(YY_BUFFER_STATE buffer);

    extern int yylineno;
    extern char* yytext;

    int Compiler_GetSyntaxErrorLine(void);
    const char* Compiler_GetSyntaxErrorMessage(void);
    const char* Compiler_GetSyntaxErrorCorrection(void);
    void Compiler_ResetSyntaxError(void);
}

/*
    نقطة الدخول للتحليل النحوي من C#.
    0 = التحليل النحوي ناجح وخالٍ من الأخطاء
    1 = خطأ نحوي Syntax Error
   -1 = خطأ في تمرير النص
*/
extern "C" __declspec(dllexport)
int __cdecl Compiler_Parse(const char* source)
{
    if (source == nullptr)
    {
        return -1;
    }

    yylineno = 1;
    Compiler_ResetSyntaxError();

    YY_BUFFER_STATE buffer = yy_scan_string(source);
    if (buffer == nullptr)
    {
        return -1;
    }

    int result = yyparse();

    yy_delete_buffer(buffer);

    return result;
}

typedef void(__cdecl* TokenCallback)(
    int token,
    const char* lexeme,
    int line
);

extern "C" __declspec(dllexport)
int __cdecl Compiler_Lex(
    const char* source,
    TokenCallback callback)
{
    if (source == nullptr || callback == nullptr)
        return -1;

    yylineno = 1;

    YY_BUFFER_STATE buffer = yy_scan_string(source);
    if (buffer == nullptr)
        return -1;

    int token;
    int count = 0;

    while ((token = yylex()) != 0)
    {
        callback(
            token,
            yytext,
            yylineno
        );
        count++;
    }

    yy_delete_buffer(buffer);
    return count;
}

extern "C" __declspec(dllexport)
const char* __cdecl Compiler_GetTokenName(int token)
{
    switch (token)
    {
    case PROGRAM: return (const char*)u8"كلمة محجوزة: برنامج";
    case CONST: return (const char*)u8"كلمة محجوزة: ثابت";
    case TYPE: return (const char*)u8"كلمة محجوزة: نوع";
    case LIST: return (const char*)u8"كلمة محجوزة: قائمة";
    case OF: return (const char*)u8"كلمة محجوزة: من";
    case RECORD: return (const char*)u8"كلمة محجوزة: سجل";
    case VAR: return (const char*)u8"كلمة محجوزة: متغير";
    case PROCEDURE: return (const char*)u8"كلمة محجوزة: إجراء";
    case BY_VALUE: return (const char*)u8"تمرير: بالقيمة";
    case BY_REFERENCE: return (const char*)u8"تمرير: بالمرجع";

    case INT_TYPE: return (const char*)u8"نوع بيانات: صحيح";
    case REAL_TYPE: return (const char*)u8"نوع بيانات: حقيقي";
    case BOOL_TYPE: return (const char*)u8"نوع بيانات: منطقي";
    case CHAR_TYPE: return (const char*)u8"نوع بيانات: محرف";
    case STRING_TYPE: return (const char*)u8"نوع بيانات: خيط رمزي";

    case READ: return (const char*)u8"تعليمة: اقرأ";
    case PRINT: return (const char*)u8"تعليمة: اطبع";

    case IF: return (const char*)u8"شرط: إذا";
    case THEN: return (const char*)u8"شرط: فإن";
    case ELSE: return (const char*)u8"شرط: وإلا";
    case SEMI_ELSE: return (const char*)u8"فاصلة منقوطة وإلا";

    case FOR: return (const char*)u8"حلقة تكرار: لكل";
    case TO: return (const char*)u8"حلقة: إلى";
    case STEP: return (const char*)u8"خطوة: بقدر";
    case WHILE: return (const char*)u8"حلقة: طالما";
    case DO: return (const char*)u8"كرر: افعل";
    case REPEAT: return (const char*)u8"حلقة: أعد";
    case UNTIL: return (const char*)u8"حلقة: حتى";

    case TRUE: return (const char*)u8"قيمة منطقية: نعم";
    case FALSE: return (const char*)u8"قيمة منطقية: لا";

    case LE: return (const char*)u8"مقارنة: أصغر من أو يساوي";
    case GE: return (const char*)u8"مقارنة: أكبر من أو يساوي";
    case EQ: return (const char*)u8"مقارنة: يساوي";
    case NE: return (const char*)u8"مقارنة: لا يساوي";
    case LT: return (const char*)u8"مقارنة: أصغر من";
    case GT: return (const char*)u8"مقارنة: أكبر من";

    case OR: return (const char*)u8"عامل منطقي: أو";
    case AND: return (const char*)u8"عامل منطقي: و";
    case NOT: return (const char*)u8"عامل منطقي: ليس";

    case PLUS: return (const char*)u8"عامل جمع (+)";
    case MINUS: return (const char*)u8"عامل طرح (-)";
    case POWER: return (const char*)u8"عامل أس (^)";
    case MUL: return (const char*)u8"عامل ضرب (*)";
    case REAL_DIV: return (const char*)u8"قسمة حقيقية (/)";
    case INT_DIV: return (const char*)u8"قسمة صحيحة (\\)";
    case MOD: return (const char*)u8"باقي القسمة (%)";
    case ASSIGN: return (const char*)u8"عامل إسناد (=)";

    case DOT: return (const char*)u8"نقطة النهاية (.)";
    case COLON: return (const char*)u8"نقطتان (:)";
    case SEMICOLON: return (const char*)u8"فاصلة منقوطة (؛)";
    case COMMA: return (const char*)u8"فاصلة (،)";

    case LBRACE: return (const char*)u8"قوس كتلة مفتوح ({)";
    case RBRACE: return (const char*)u8"قوس كتلة مغلق (})";
    case LBRACKET: return (const char*)u8"قوس مصفوفة مفتوح ([)";
    case RBRACKET: return (const char*)u8"قوس مصفوفة مغلق (])";
    case LPAREN: return (const char*)u8"قوس دائري مفتوح (()";
    case RPAREN: return (const char*)u8"قوس دائري مغلق ())";

    case REAL_LITERAL: return (const char*)u8"عدد حقيقي";
    case INTEGER_LITERAL: return (const char*)u8"عدد صحيح";
    case STRING_LITERAL: return (const char*)u8"خيط رمزي";
    case CHAR_LITERAL: return (const char*)u8"محرف";
    case IDENTIFIER: return (const char*)u8"معرّف";

    case LEXICAL_ERROR: return (const char*)u8"خطأ لغوي";

    default: return (const char*)u8"رمز غير معروف";
    }
}