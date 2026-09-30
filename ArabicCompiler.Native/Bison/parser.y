%{
#include <stdio.h>
#include <stdlib.h>
#include <string.h>

extern int yylex(void);
extern int yylineno;
extern char* yytext;

void yyerror(const char* message);

/*
    معلومات آخر خطأ نحوي.
    أضيفت فقط لتمرير الخطأ إلى واجهة C# بدون تغيير أي قاعدة نحوية.
*/
static int g_syntax_error_line = 0;
static char g_syntax_error_message[1024] = "";
static char g_syntax_error_correction[1024] = "";

/*
    يحول رسالة Bison إلى تصحيح مقترح بالعربية.
*/
static int is_token_name_char(char c)
{
    return
        (c >= 'A' && c <= 'Z') ||
        (c >= '0' && c <= '9') ||
        c == '_';
}

static int expected_has(const char* message, const char* token)
{
    const char* expected;
    const char* p;
    size_t len;

    if (message == NULL || token == NULL)
        return 0;

    expected = strstr(message, "expecting ");

    if (expected == NULL)
        return 0;

    expected += strlen("expecting ");
    len = strlen(token);
    p = expected;

    while ((p = strstr(p, token)) != NULL)
    {
        char before = (p == expected) ? '\0' : p[-1];
        char after = p[len];

        if (!is_token_name_char(before) &&
            !is_token_name_char(after))
        {
            return 1;
        }

        p += len;
    }

    return 0;
}

static void build_syntax_correction(const char* message)
{
    g_syntax_error_correction[0] = '\0';

    if (message == NULL)
    {
        snprintf(
            g_syntax_error_correction,
            sizeof(g_syntax_error_correction),
            u8"التصحيح المقترح: راجع صياغة التعليمة في هذا السطر."
        );
        return;
    }

    /*
        أولاً: معالجة نهاية الملف.
        نعطي الأولوية للأقواس المفقودة قبل بقية الرموز.
    */
    if (strstr(message, "unexpected end of file") != NULL)
    {
        if (expected_has(message, "RBRACE"))
        {
            snprintf(
                g_syntax_error_correction,
                sizeof(g_syntax_error_correction),
                u8"التصحيح المقترح: أضف القوس المعقوف المغلق (}) قبل نهاية البرنامج."
            );
        }
        else if (expected_has(message, "RPAREN"))
        {
            snprintf(
                g_syntax_error_correction,
                sizeof(g_syntax_error_correction),
                u8"التصحيح المقترح: أضف القوس الدائري المغلق ())."
            );
        }
        else if (expected_has(message, "RBRACKET"))
        {
            snprintf(
                g_syntax_error_correction,
                sizeof(g_syntax_error_correction),
                u8"التصحيح المقترح: أضف القوس المربع المغلق (])."
            );
        }
        else if (expected_has(message, "DOT"))
        {
            snprintf(
                g_syntax_error_correction,
                sizeof(g_syntax_error_correction),
                u8"التصحيح المقترح: أضف النقطة (.) في نهاية البرنامج."
            );
        }
        else if (expected_has(message, "SEMICOLON"))
        {
            snprintf(
                g_syntax_error_correction,
                sizeof(g_syntax_error_correction),
                u8"التصحيح المقترح: أضف الفاصلة المنقوطة العربية (؛)."
            );
        }
        else
        {
            snprintf(
                g_syntax_error_correction,
                sizeof(g_syntax_error_correction),
                u8"التصحيح المقترح: البرنامج انتهى قبل اكتمال البنية النحوية المطلوبة."
            );
        }

        return;
    }

    /*
        إذا وصلنا إلى النقطة بينما توجد أقواس لم تغلق،
        فإغلاق الأقواس يجب أن يسبق النقطة.
    */
    if (yytext != NULL && strcmp(yytext, ".") == 0)
    {
        if (expected_has(message, "RBRACE"))
        {
            snprintf(
                g_syntax_error_correction,
                sizeof(g_syntax_error_correction),
                u8"التصحيح المقترح: أضف القوس المعقوف المغلق (}) قبل النقطة (.)."
            );
            return;
        }

        if (expected_has(message, "RPAREN"))
        {
            snprintf(
                g_syntax_error_correction,
                sizeof(g_syntax_error_correction),
                u8"التصحيح المقترح: أضف القوس الدائري المغلق ()) قبل النقطة (.)."
            );
            return;
        }

        if (expected_has(message, "RBRACKET"))
        {
            snprintf(
                g_syntax_error_correction,
                sizeof(g_syntax_error_correction),
                u8"التصحيح المقترح: أضف القوس المربع المغلق (]) قبل النقطة (.)."
            );
            return;
        }

        if (expected_has(message, "SEMICOLON"))
        {
            snprintf(
                g_syntax_error_correction,
                sizeof(g_syntax_error_correction),
                u8"التصحيح المقترح: أضف الفاصلة المنقوطة العربية (؛) قبل النقطة."
            );
            return;
        }
    }

    /*
        إذا أغلق المستخدم الكتلة مباشرة،
        لكن توجد فاصلة منقوطة مطلوبة قبلها.
    */
    if (yytext != NULL && strcmp(yytext, "}") == 0)
    {
        if (expected_has(message, "SEMICOLON"))
        {
            snprintf(
                g_syntax_error_correction,
                sizeof(g_syntax_error_correction),
                u8"التصحيح المقترح: أضف الفاصلة المنقوطة العربية (؛) قبل القوس (})."
            );
            return;
        }

        if (expected_has(message, "RPAREN"))
        {
            snprintf(
                g_syntax_error_correction,
                sizeof(g_syntax_error_correction),
                u8"التصحيح المقترح: أغلق القوس الدائري ()) قبل إغلاق الكتلة."
            );
            return;
        }

        if (expected_has(message, "RBRACKET"))
        {
            snprintf(
                g_syntax_error_correction,
                sizeof(g_syntax_error_correction),
                u8"التصحيح المقترح: أغلق القوس المربع (]) قبل إغلاق الكتلة."
            );
            return;
        }
    }

    /*
        حالة شائعة:
        Bison قد يعرض أكثر من رمز متوقع مثل:
        expecting SEMICOLON or RBRACE
    */
    if (expected_has(message, "SEMICOLON") &&
        expected_has(message, "RBRACE"))
    {
        snprintf(
            g_syntax_error_correction,
            sizeof(g_syntax_error_correction),
            u8"التصحيح المقترح: أضف (؛) إذا كنت تريد كتابة تعليمة أخرى، أو أغلق الكتلة بالقوس (}) إذا انتهت التعليمات."
        );
        return;
    }

    if (expected_has(message, "SEMICOLON"))
    {
        snprintf(
            g_syntax_error_correction,
            sizeof(g_syntax_error_correction),
            u8"التصحيح المقترح: أضف الفاصلة المنقوطة العربية (؛) في الموضع المطلوب."
        );
    }
    else if (expected_has(message, "RBRACE"))
    {
        snprintf(
            g_syntax_error_correction,
            sizeof(g_syntax_error_correction),
            u8"التصحيح المقترح: أضف القوس المعقوف المغلق (})."
        );
    }
    else if (expected_has(message, "LBRACE"))
    {
        snprintf(
            g_syntax_error_correction,
            sizeof(g_syntax_error_correction),
            u8"التصحيح المقترح: أضف القوس المعقوف المفتوح ({)."
        );
    }
    else if (expected_has(message, "RPAREN"))
    {
        snprintf(
            g_syntax_error_correction,
            sizeof(g_syntax_error_correction),
            u8"التصحيح المقترح: أضف القوس الدائري المغلق ())."
        );
    }
    else if (expected_has(message, "LPAREN"))
    {
        snprintf(
            g_syntax_error_correction,
            sizeof(g_syntax_error_correction),
            u8"التصحيح المقترح: أضف القوس الدائري المفتوح (()."
        );
    }
    else if (expected_has(message, "RBRACKET"))
    {
        snprintf(
            g_syntax_error_correction,
            sizeof(g_syntax_error_correction),
            u8"التصحيح المقترح: أضف القوس المربع المغلق (])."
        );
    }
    else if (expected_has(message, "LBRACKET"))
    {
        snprintf(
            g_syntax_error_correction,
            sizeof(g_syntax_error_correction),
            u8"التصحيح المقترح: أضف القوس المربع المفتوح ([)."
        );
    }
    else if (expected_has(message, "DOT"))
    {
        snprintf(
            g_syntax_error_correction,
            sizeof(g_syntax_error_correction),
            u8"التصحيح المقترح: أضف النقطة (.) في نهاية البرنامج."
        );
    }
    else if (expected_has(message, "ASSIGN"))
    {
        snprintf(
            g_syntax_error_correction,
            sizeof(g_syntax_error_correction),
            u8"التصحيح المقترح: أضف عامل الإسناد (=) في الموضع المطلوب."
        );
    }
    else if (expected_has(message, "COLON"))
    {
        snprintf(
            g_syntax_error_correction,
            sizeof(g_syntax_error_correction),
            u8"التصحيح المقترح: أضف النقطتين (:) في الموضع المطلوب."
        );
    }
    else if (expected_has(message, "COMMA"))
    {
        snprintf(
            g_syntax_error_correction,
            sizeof(g_syntax_error_correction),
            u8"التصحيح المقترح: أضف الفاصلة (,) في الموضع المطلوب."
        );
    }
    else if (expected_has(message, "THEN"))
    {
        snprintf(
            g_syntax_error_correction,
            sizeof(g_syntax_error_correction),
            u8"التصحيح المقترح: أضف الكلمة المحجوزة (فان) بعد الشرط."
        );
    }
    else if (expected_has(message, "TO"))
    {
        snprintf(
            g_syntax_error_correction,
            sizeof(g_syntax_error_correction),
            u8"التصحيح المقترح: أضف الكلمة المحجوزة (الى) في مجال التكرار."
        );
    }
    else if (expected_has(message, "DO"))
    {
        snprintf(
            g_syntax_error_correction,
            sizeof(g_syntax_error_correction),
            u8"التصحيح المقترح: أضف الكلمة المحجوزة (استمر) بعد شرط طالما."
        );
    }
    else if (expected_has(message, "OF"))
    {
        snprintf(
            g_syntax_error_correction,
            sizeof(g_syntax_error_correction),
            u8"التصحيح المقترح: أضف الكلمة المحجوزة (من) في تعريف القائمة."
        );
    }
    else if (expected_has(message, "IDENTIFIER"))
    {
        snprintf(
            g_syntax_error_correction,
            sizeof(g_syntax_error_correction),
            u8"التصحيح المقترح: أضف اسماً معرفاً صحيحاً في هذا الموضع."
        );
    }
    else if (expected_has(message, "INTEGER_LITERAL"))
    {
        snprintf(
            g_syntax_error_correction,
            sizeof(g_syntax_error_correction),
            u8"التصحيح المقترح: أضف عدداً صحيحاً في هذا الموضع."
        );
    }
    else if (expected_has(message, "REAL_LITERAL"))
    {
        snprintf(
            g_syntax_error_correction,
            sizeof(g_syntax_error_correction),
            u8"التصحيح المقترح: أضف عدداً حقيقياً في هذا الموضع."
        );
    }
    else if (expected_has(message, "STRING_LITERAL"))
    {
        snprintf(
            g_syntax_error_correction,
            sizeof(g_syntax_error_correction),
            u8"التصحيح المقترح: أضف خيطاً رمزياً صحيحاً في هذا الموضع."
        );
    }
    else if (expected_has(message, "CHAR_LITERAL"))
    {
        snprintf(
            g_syntax_error_correction,
            sizeof(g_syntax_error_correction),
            u8"التصحيح المقترح: أضف قيمة حرفية صحيحة في هذا الموضع."
        );
    }
    else
    {
        snprintf(
            g_syntax_error_correction,
            sizeof(g_syntax_error_correction),
            u8"التصحيح المقترح: الرمز الحالي غير صالح في هذا الموضع؛ راجع ترتيب الرموز والكلمات حسب قواعد اللغة."
        );
    }
}

/*
    دوال يقرأ منها C# تفاصيل آخر خطأ نحوي.
*/
__declspec(dllexport)
int __cdecl Compiler_GetSyntaxErrorLine(void)
{
    return g_syntax_error_line;
}

__declspec(dllexport)
const char* __cdecl Compiler_GetSyntaxErrorMessage(void)
{
    return g_syntax_error_message;
}

__declspec(dllexport)
const char* __cdecl Compiler_GetSyntaxErrorCorrection(void)
{
    return g_syntax_error_correction;
}

__declspec(dllexport)
void __cdecl Compiler_ResetSyntaxError(void)
{
    g_syntax_error_line = 0;
    g_syntax_error_message[0] = '\0';
    g_syntax_error_correction[0] = '\0';
}
%}

%define parse.error verbose


/* =====================================================
   القيم القادمة من Flex
   ===================================================== */

%union
{
    char* text;
}


/* =====================================================
   الكلمات المحجوزة
   ===================================================== */

%token PROGRAM
%token CONST
%token TYPE
%token LIST
%token OF
%token RECORD
%token VAR
%token PROCEDURE
%token BY_VALUE
%token BY_REFERENCE

%token INT_TYPE
%token REAL_TYPE
%token BOOL_TYPE
%token CHAR_TYPE
%token STRING_TYPE

%token READ
%token PRINT

%token IF
%token THEN
%token ELSE

%token FOR
%token TO
%token STEP

%token WHILE
%token DO

%token REPEAT
%token UNTIL

%token TRUE
%token FALSE


/* =====================================================
   المعاملات
   ===================================================== */

%token LT
%token GT
%token LE
%token GE
%token EQ
%token NE

%token PLUS
%token MINUS
%token OR

%token POWER
%token MUL
%token REAL_DIV
%token INT_DIV
%token MOD
%token AND

%token NOT
%token ASSIGN


/* =====================================================
   العلامات والفواصل
   ===================================================== */

%token DOT
%token COLON

%token SEMICOLON

/*
   Token داخلي يمثل:
   ؛ والا

   سيقوم Flex بإرجاعه لاحقاً.
*/
%token SEMI_ELSE

%token COMMA

%token LBRACE
%token RBRACE

%token LBRACKET
%token RBRACKET

%token LPAREN
%token RPAREN


/* =====================================================
   Tokens التي تحمل قيمة
   ===================================================== */

%token <text> IDENTIFIER
%token <text> INTEGER_LITERAL
%token <text> REAL_LITERAL
%token <text> STRING_LITERAL
%token <text> CHAR_LITERAL

%token LEXICAL_ERROR


/* =====================================================
   أولوية اذا / والا

   ELSE و SEMI_ELSE أعلى من THEN
   ===================================================== */

%precedence THEN
%precedence ELSE SEMI_ELSE


%start program


%%


/* =====================================================
   1. البرنامج

   <برنامج> :=
   برنامج <اسم_برنامج> ؛ <كتلة_برمجية> .
   ===================================================== */

program
    : PROGRAM program_name SEMICOLON program_block DOT
    ;


/* =====================================================
   2. الكتلة البرمجية
   ===================================================== */

program_block
    : declaration_part_opt instruction_list
    ;


declaration_part_opt
    : %empty
    | declaration_part
    ;


/*
   يجب أن يحتوي جزء التعريفات على قسم واحد
   على الأقل عندما يكون موجوداً.
*/

declaration_part
    : constant_definitions
      type_definitions_opt
      variable_definitions_opt
      procedure_definitions_opt

    | type_definitions
      variable_definitions_opt
      procedure_definitions_opt

    | variable_definitions
      procedure_definitions_opt

    | procedure_definitions
    ;


/* =====================================================
   3. تعريف الثوابت
   ===================================================== */

constant_definitions
    : CONST constant_definition constant_definition_tail
    ;


constant_definition_tail
    : %empty
    | constant_definition_tail constant_definition
    ;


constant_definition
    : constant_name ASSIGN constant_value SEMICOLON
    ;


/* =====================================================
   4. تعريف الأنواع
   ===================================================== */

type_definitions_opt
    : %empty
    | type_definitions
    ;


type_definitions
    : TYPE type_definition type_definition_tail
    ;


type_definition_tail
    : %empty
    | type_definition_tail type_definition
    ;


type_definition
    : type_name ASSIGN compound_type SEMICOLON
    ;


compound_type
    : list_type
    | record_type
    ;


/*
   القواعد الأصلية تستخدم <عدد>
   في حجم القائمة بدون تعريف مستقل.

   نستخدم INTEGER_LITERAL كحجم القائمة.
*/

list_type
    : LIST LBRACKET INTEGER_LITERAL RBRACKET OF data_type
    ;


/* =====================================================
   5. نوع السجل
   ===================================================== */

record_type
    : RECORD LBRACE field_list RBRACE
    ;


field_list
    : field_definition field_definition_tail
    ;


field_definition_tail
    : %empty
    | field_definition_tail SEMICOLON field_definition
    ;


field_definition
    : field_name identifier_tail COLON data_type
    ;


identifier_tail
    : %empty
    | identifier_tail COMMA IDENTIFIER
    ;


/* =====================================================
   6. تعريف المتغيرات
   ===================================================== */

variable_definitions_opt
    : %empty
    | variable_definitions
    ;


variable_definitions
    : VAR variable_definition variable_definition_tail
    ;


variable_definition_tail
    : %empty
    | variable_definition_tail variable_definition
    ;


variable_definition
    : variable_group SEMICOLON
    ;


variable_group
    : variable_name variable_name_tail COLON data_type
    ;


variable_name_tail
    : %empty
    | variable_name_tail COMMA variable_name
    ;


/* =====================================================
   7. تعريف الإجراءات
   ===================================================== */

procedure_definitions_opt
    : %empty
    | procedure_definitions
    ;


procedure_definitions
    : procedure_definition procedure_definition_tail
    ;


procedure_definition_tail
    : %empty
    | procedure_definition_tail procedure_definition
    ;


procedure_definition
    : procedure_header procedure_block SEMICOLON
    ;


procedure_header
    : PROCEDURE
      procedure_name
      LPAREN
      formal_parameters_opt
      RPAREN
      SEMICOLON
    ;


procedure_block
    : program_block
    ;


formal_parameters_opt
    : %empty
    | formal_parameters
    ;


formal_parameters
    : formal_parameter formal_parameter_tail
    ;


formal_parameter_tail
    : %empty
    | formal_parameter_tail SEMICOLON formal_parameter
    ;


formal_parameter
    : pass_mode_opt variable_group
    ;


pass_mode_opt
    : %empty
    | BY_VALUE
    | BY_REFERENCE
    ;


/* =====================================================
   8. أنواع البيانات
   ===================================================== */

data_type
    : INT_TYPE
    | REAL_TYPE
    | BOOL_TYPE
    | CHAR_TYPE
    | STRING_TYPE
    | type_name
    ;


/* =====================================================
   9. قائمة التعليمات
   ===================================================== */

instruction_list
    : LBRACE instruction instruction_tail RBRACE
    ;


instruction_tail
    : %empty
    | instruction_tail SEMICOLON instruction
    ;


instruction
    : assignment_statement
    | input_statement
    | output_statement
    | procedure_call
    | conditional_statement
    | repetition_statement
    | instruction_list
    | %empty
    ;


/*
   %empty تمثل <فراغ>
   الموجودة في قواعد الدكتور.
*/


/* =====================================================
   10. تعليمة الإسناد
   ===================================================== */

assignment_statement
    : variable_access ASSIGN expression
    ;


/* =====================================================
   11. تعليمة الإدخال
   ===================================================== */

input_statement
    : READ LPAREN variable_access RPAREN
    ;


/* =====================================================
   12. تعليمة الإخراج
   ===================================================== */

output_statement
    : PRINT LPAREN print_list RPAREN
    ;


print_list
    : print_item print_item_tail
    ;


print_item_tail
    : %empty
    | print_item_tail COMMA print_item
    ;


print_item
    : variable_access
    | literal
    ;


/* =====================================================
   13. استدعاء الإجراءات
   ===================================================== */

procedure_call
    : procedure_name LPAREN actual_parameters_opt RPAREN
    ;


actual_parameters_opt
    : %empty
    | actual_parameters
    ;


actual_parameters
    : actual_parameter actual_parameter_tail
    ;


actual_parameter_tail
    : %empty
    | actual_parameter_tail COMMA actual_parameter
    ;


/*
   <معلمة_حقيقية> في المصدر:
   <تعبير> | <متغير_وصول>

   ومتغير الوصول يدخل أصلاً في التعبير.
*/

actual_parameter
    : expression
    ;


/* =====================================================
   14. تعليمات الشرط
   ===================================================== */


/*
   وضع %prec THEN هنا يحل مشكلة Dangling Else.

   إذا ظهر ELSE أو SEMI_ELSE بعد if_statement
   فإن Bison يختار متابعة الجملة بدلاً من
   تقليل if_statement مباشرة.
*/

conditional_statement
    : if_statement %prec THEN
    | if_else_statement
    | if_else_if_statement
    ;


/* -------------------------
   اذا
   ------------------------- */

if_statement
    : IF LPAREN condition RPAREN THEN instruction
    ;


condition
    : expression
    ;


/* -------------------------
   اذا ... والا
   ------------------------- */

if_else_statement
    : if_statement ELSE instruction
    ;


/* -------------------------
   اذا ... ؛ والا اذا ...
   ------------------------- */

if_else_if_statement
    : if_statement
      SEMI_ELSE
      if_statement
      else_if_tail
      optional_else
    ;


/*
   المزيد من:
   ؛ والا اذا
*/

else_if_tail
    : %empty
    | else_if_tail SEMI_ELSE if_statement
    ;


/*
   والا النهائية اختيارية.

   نعطي الفراغ أولوية THEN حتى يتم Shift
   عند وجود ELSE.
*/

optional_else
    : %empty %prec THEN
    | ELSE instruction
    ;


/* =====================================================
   15. تعليمات التكرار
   ===================================================== */

repetition_statement
    : for_statement
    | while_statement
    | repeat_until_statement
    ;


/* -------------------------
   كرر
   ------------------------- */

for_statement
    : FOR LPAREN repetition_range RPAREN instruction
    ;


repetition_range
    : variable_name
      ASSIGN
      expression
      TO
      expression
      step_opt
    ;


step_opt
    : %empty
    | STEP expression
    ;


/* -------------------------
   طالما
   ------------------------- */

while_statement
    : WHILE LPAREN condition RPAREN DO instruction
    ;


/* -------------------------
   اعد ... حتى
   ------------------------- */

repeat_until_statement
    : REPEAT instruction UNTIL LPAREN condition RPAREN
    ;


/* =====================================================
   16. التعبيرات
   ===================================================== */

expression
    : simple_expression
    | simple_expression relational_operator simple_expression
    ;


relational_operator
    : LT
    | GT
    | LE
    | GE
    | EQ
    | NE
    ;


simple_expression
    : sign_opt term addition_tail
    ;


sign_opt
    : %empty
    | sign_operator
    ;


sign_operator
    : PLUS
    | MINUS
    ;


addition_tail
    : %empty
    | addition_tail addition_operator term
    ;


addition_operator
    : sign_operator
    | OR
    ;


term
    : factor multiplication_tail
    ;


multiplication_tail
    : %empty
    | multiplication_tail multiplication_operator factor
    ;


multiplication_operator
    : POWER
    | MUL
    | REAL_DIV
    | INT_DIV
    | MOD
    | AND
    ;


/* =====================================================
   17. العامل
   ===================================================== */

factor
    : numeric
    | literal
    | boolean_value
    | identifier_factor
    | LPAREN expression RPAREN
    | NOT factor
    ;


/*
   Lexer يعيد جميع الأسماء كـ IDENTIFIER.

   معرفة هل الاسم:
   متغير
   ثابت
   نوع

   ستكون في Semantic Analysis لاحقاً.
*/

identifier_factor
    : IDENTIFIER
    | IDENTIFIER selector
    ;


/* =====================================================
   18. متغير الوصول
   ===================================================== */

variable_access
    : IDENTIFIER
    | IDENTIFIER selector
    ;


selector
    : indexed_selector
    | field_selector
    ;


indexed_selector
    : LBRACKET expression RBRACKET
    ;


field_selector
    : DOT field_name
    ;


/* =====================================================
   19. القيم الثابتة
   ===================================================== */

constant_value
    : numeric
    | literal
    | boolean_value
    | constant_name
    ;


numeric
    : INTEGER_LITERAL
    | REAL_LITERAL
    ;


literal
    : STRING_LITERAL
    | CHAR_LITERAL
    ;


boolean_value
    : TRUE
    | FALSE
    ;


/* =====================================================
   20. الأسماء والمعرفات
   ===================================================== */

program_name
    : IDENTIFIER
    ;


constant_name
    : IDENTIFIER
    ;


type_name
    : IDENTIFIER
    ;


field_name
    : IDENTIFIER
    ;


variable_name
    : IDENTIFIER
    ;


procedure_name
    : IDENTIFIER
    ;


%%


/* =====================================================
   معالجة أخطاء Syntax
   ===================================================== */

void yyerror(const char* message)
{
    g_syntax_error_line = yylineno;

    /*
        وصف الخطأ بالعربية.
        إذا وصل Bison إلى نهاية الملف قبل اكتمال القاعدة
        نعطي رسالة أوضح، وإلا نعرض الرمز الموجود عند موضع الخطأ.
    */
    if (message != NULL &&
        strstr(message, "unexpected end of file") != NULL)
    {
        if (expected_has(message, "DOT"))
        {
            snprintf(
                g_syntax_error_message,
                sizeof(g_syntax_error_message),
                u8"وصل المترجم إلى نهاية البرنامج ولم يجد النقطة '.' الإلزامية في نهاية البرنامج."
            );
        }
        else
        {
            snprintf(
                g_syntax_error_message,
                sizeof(g_syntax_error_message),
                u8"وصل المترجم إلى نهاية البرنامج قبل اكتمال الصياغة النحوية."
            );
        }
    }
    else if (message != NULL &&
             strstr(message, "LEXICAL_ERROR") != NULL)
    {
        snprintf(
            g_syntax_error_message,
            sizeof(g_syntax_error_message),
            u8"يوجد رمز غير صالح لغوياً عند هذا الموضع."
        );
    }
    else if (yytext != NULL && yytext[0] != '\0')
    {
        snprintf(
            g_syntax_error_message,
            sizeof(g_syntax_error_message),
            u8"رمز غير متوقع عند التحليل النحوي: [%s]",
            yytext
        );
    }
    else
    {
        snprintf(
            g_syntax_error_message,
            sizeof(g_syntax_error_message),
            u8"يوجد خطأ في الصياغة النحوية للبرنامج."
        );
    }

    build_syntax_correction(message);

    fprintf(
        stderr,
        "Syntax Error at line %d: %s\n",
        yylineno,
        message != NULL ? message : "syntax error"
    );
}