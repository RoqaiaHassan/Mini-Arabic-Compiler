/* A Bison parser, made by GNU Bison 3.8.2.  */

/* Bison implementation for Yacc-like parsers in C

   Copyright (C) 1984, 1989-1990, 2000-2015, 2018-2021 Free Software Foundation,
   Inc.

   This program is free software: you can redistribute it and/or modify
   it under the terms of the GNU General Public License as published by
   the Free Software Foundation, either version 3 of the License, or
   (at your option) any later version.

   This program is distributed in the hope that it will be useful,
   but WITHOUT ANY WARRANTY; without even the implied warranty of
   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
   GNU General Public License for more details.

   You should have received a copy of the GNU General Public License
   along with this program.  If not, see <https://www.gnu.org/licenses/>.  */

/* As a special exception, you may create a larger work that contains
   part or all of the Bison parser skeleton and distribute that work
   under terms of your choice, so long as that work isn't itself a
   parser generator using the skeleton or a modified version thereof
   as a parser skeleton.  Alternatively, if you modify or redistribute
   the parser skeleton itself, you may (at your option) remove this
   special exception, which will cause the skeleton and the resulting
   Bison output files to be licensed under the GNU General Public
   License without this special exception.

   This special exception was added by the Free Software Foundation in
   version 2.2 of Bison.  */

/* C LALR(1) parser skeleton written by Richard Stallman, by
   simplifying the original so-called "semantic" parser.  */

/* DO NOT RELY ON FEATURES THAT ARE NOT DOCUMENTED in the manual,
   especially those whose name start with YY_ or yy_.  They are
   private implementation details that can be changed or removed.  */

/* All symbols defined below should begin with yy or YY, to avoid
   infringing on user name space.  This should be done even for local
   variables, as they might otherwise be expanded by user macros.
   There are some unavoidable exceptions within include files to
   define necessary library symbols; they are noted "INFRINGES ON
   USER NAME SPACE" below.  */

/* Identify Bison output, and Bison version.  */
#define YYBISON 30802

/* Bison version string.  */
#define YYBISON_VERSION "3.8.2"

/* Skeleton name.  */
#define YYSKELETON_NAME "yacc.c"

/* Pure parsers.  */
#define YYPURE 0

/* Push parsers.  */
#define YYPUSH 0

/* Pull parsers.  */
#define YYPULL 1




/* First part of user prologue.  */
#line 1 "D:\\ArabicCompiler\\ArabicCompiler\\ArabicCompiler.Native\\Bison\\parser.y"

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

#line 500 "D:\\ArabicCompiler\\ArabicCompiler\\ArabicCompiler.Native\\Bison\\parser.tab.c"

# ifndef YY_CAST
#  ifdef __cplusplus
#   define YY_CAST(Type, Val) static_cast<Type> (Val)
#   define YY_REINTERPRET_CAST(Type, Val) reinterpret_cast<Type> (Val)
#  else
#   define YY_CAST(Type, Val) ((Type) (Val))
#   define YY_REINTERPRET_CAST(Type, Val) ((Type) (Val))
#  endif
# endif
# ifndef YY_NULLPTR
#  if defined __cplusplus
#   if 201103L <= __cplusplus
#    define YY_NULLPTR nullptr
#   else
#    define YY_NULLPTR 0
#   endif
#  else
#   define YY_NULLPTR ((void*)0)
#  endif
# endif

#include "parser.tab.h"
/* Symbol kind.  */
enum yysymbol_kind_t
{
  YYSYMBOL_YYEMPTY = -2,
  YYSYMBOL_YYEOF = 0,                      /* "end of file"  */
  YYSYMBOL_YYerror = 1,                    /* error  */
  YYSYMBOL_YYUNDEF = 2,                    /* "invalid token"  */
  YYSYMBOL_PROGRAM = 3,                    /* PROGRAM  */
  YYSYMBOL_CONST = 4,                      /* CONST  */
  YYSYMBOL_TYPE = 5,                       /* TYPE  */
  YYSYMBOL_LIST = 6,                       /* LIST  */
  YYSYMBOL_OF = 7,                         /* OF  */
  YYSYMBOL_RECORD = 8,                     /* RECORD  */
  YYSYMBOL_VAR = 9,                        /* VAR  */
  YYSYMBOL_PROCEDURE = 10,                 /* PROCEDURE  */
  YYSYMBOL_BY_VALUE = 11,                  /* BY_VALUE  */
  YYSYMBOL_BY_REFERENCE = 12,              /* BY_REFERENCE  */
  YYSYMBOL_INT_TYPE = 13,                  /* INT_TYPE  */
  YYSYMBOL_REAL_TYPE = 14,                 /* REAL_TYPE  */
  YYSYMBOL_BOOL_TYPE = 15,                 /* BOOL_TYPE  */
  YYSYMBOL_CHAR_TYPE = 16,                 /* CHAR_TYPE  */
  YYSYMBOL_STRING_TYPE = 17,               /* STRING_TYPE  */
  YYSYMBOL_READ = 18,                      /* READ  */
  YYSYMBOL_PRINT = 19,                     /* PRINT  */
  YYSYMBOL_IF = 20,                        /* IF  */
  YYSYMBOL_THEN = 21,                      /* THEN  */
  YYSYMBOL_ELSE = 22,                      /* ELSE  */
  YYSYMBOL_FOR = 23,                       /* FOR  */
  YYSYMBOL_TO = 24,                        /* TO  */
  YYSYMBOL_STEP = 25,                      /* STEP  */
  YYSYMBOL_WHILE = 26,                     /* WHILE  */
  YYSYMBOL_DO = 27,                        /* DO  */
  YYSYMBOL_REPEAT = 28,                    /* REPEAT  */
  YYSYMBOL_UNTIL = 29,                     /* UNTIL  */
  YYSYMBOL_TRUE = 30,                      /* TRUE  */
  YYSYMBOL_FALSE = 31,                     /* FALSE  */
  YYSYMBOL_LT = 32,                        /* LT  */
  YYSYMBOL_GT = 33,                        /* GT  */
  YYSYMBOL_LE = 34,                        /* LE  */
  YYSYMBOL_GE = 35,                        /* GE  */
  YYSYMBOL_EQ = 36,                        /* EQ  */
  YYSYMBOL_NE = 37,                        /* NE  */
  YYSYMBOL_PLUS = 38,                      /* PLUS  */
  YYSYMBOL_MINUS = 39,                     /* MINUS  */
  YYSYMBOL_OR = 40,                        /* OR  */
  YYSYMBOL_POWER = 41,                     /* POWER  */
  YYSYMBOL_MUL = 42,                       /* MUL  */
  YYSYMBOL_REAL_DIV = 43,                  /* REAL_DIV  */
  YYSYMBOL_INT_DIV = 44,                   /* INT_DIV  */
  YYSYMBOL_MOD = 45,                       /* MOD  */
  YYSYMBOL_AND = 46,                       /* AND  */
  YYSYMBOL_NOT = 47,                       /* NOT  */
  YYSYMBOL_ASSIGN = 48,                    /* ASSIGN  */
  YYSYMBOL_DOT = 49,                       /* DOT  */
  YYSYMBOL_COLON = 50,                     /* COLON  */
  YYSYMBOL_SEMICOLON = 51,                 /* SEMICOLON  */
  YYSYMBOL_SEMI_ELSE = 52,                 /* SEMI_ELSE  */
  YYSYMBOL_COMMA = 53,                     /* COMMA  */
  YYSYMBOL_LBRACE = 54,                    /* LBRACE  */
  YYSYMBOL_RBRACE = 55,                    /* RBRACE  */
  YYSYMBOL_LBRACKET = 56,                  /* LBRACKET  */
  YYSYMBOL_RBRACKET = 57,                  /* RBRACKET  */
  YYSYMBOL_LPAREN = 58,                    /* LPAREN  */
  YYSYMBOL_RPAREN = 59,                    /* RPAREN  */
  YYSYMBOL_IDENTIFIER = 60,                /* IDENTIFIER  */
  YYSYMBOL_INTEGER_LITERAL = 61,           /* INTEGER_LITERAL  */
  YYSYMBOL_REAL_LITERAL = 62,              /* REAL_LITERAL  */
  YYSYMBOL_STRING_LITERAL = 63,            /* STRING_LITERAL  */
  YYSYMBOL_CHAR_LITERAL = 64,              /* CHAR_LITERAL  */
  YYSYMBOL_LEXICAL_ERROR = 65,             /* LEXICAL_ERROR  */
  YYSYMBOL_YYACCEPT = 66,                  /* $accept  */
  YYSYMBOL_program = 67,                   /* program  */
  YYSYMBOL_program_block = 68,             /* program_block  */
  YYSYMBOL_declaration_part_opt = 69,      /* declaration_part_opt  */
  YYSYMBOL_declaration_part = 70,          /* declaration_part  */
  YYSYMBOL_constant_definitions = 71,      /* constant_definitions  */
  YYSYMBOL_constant_definition_tail = 72,  /* constant_definition_tail  */
  YYSYMBOL_constant_definition = 73,       /* constant_definition  */
  YYSYMBOL_type_definitions_opt = 74,      /* type_definitions_opt  */
  YYSYMBOL_type_definitions = 75,          /* type_definitions  */
  YYSYMBOL_type_definition_tail = 76,      /* type_definition_tail  */
  YYSYMBOL_type_definition = 77,           /* type_definition  */
  YYSYMBOL_compound_type = 78,             /* compound_type  */
  YYSYMBOL_list_type = 79,                 /* list_type  */
  YYSYMBOL_record_type = 80,               /* record_type  */
  YYSYMBOL_field_list = 81,                /* field_list  */
  YYSYMBOL_field_definition_tail = 82,     /* field_definition_tail  */
  YYSYMBOL_field_definition = 83,          /* field_definition  */
  YYSYMBOL_identifier_tail = 84,           /* identifier_tail  */
  YYSYMBOL_variable_definitions_opt = 85,  /* variable_definitions_opt  */
  YYSYMBOL_variable_definitions = 86,      /* variable_definitions  */
  YYSYMBOL_variable_definition_tail = 87,  /* variable_definition_tail  */
  YYSYMBOL_variable_definition = 88,       /* variable_definition  */
  YYSYMBOL_variable_group = 89,            /* variable_group  */
  YYSYMBOL_variable_name_tail = 90,        /* variable_name_tail  */
  YYSYMBOL_procedure_definitions_opt = 91, /* procedure_definitions_opt  */
  YYSYMBOL_procedure_definitions = 92,     /* procedure_definitions  */
  YYSYMBOL_procedure_definition_tail = 93, /* procedure_definition_tail  */
  YYSYMBOL_procedure_definition = 94,      /* procedure_definition  */
  YYSYMBOL_procedure_header = 95,          /* procedure_header  */
  YYSYMBOL_procedure_block = 96,           /* procedure_block  */
  YYSYMBOL_formal_parameters_opt = 97,     /* formal_parameters_opt  */
  YYSYMBOL_formal_parameters = 98,         /* formal_parameters  */
  YYSYMBOL_formal_parameter_tail = 99,     /* formal_parameter_tail  */
  YYSYMBOL_formal_parameter = 100,         /* formal_parameter  */
  YYSYMBOL_pass_mode_opt = 101,            /* pass_mode_opt  */
  YYSYMBOL_data_type = 102,                /* data_type  */
  YYSYMBOL_instruction_list = 103,         /* instruction_list  */
  YYSYMBOL_instruction_tail = 104,         /* instruction_tail  */
  YYSYMBOL_instruction = 105,              /* instruction  */
  YYSYMBOL_assignment_statement = 106,     /* assignment_statement  */
  YYSYMBOL_input_statement = 107,          /* input_statement  */
  YYSYMBOL_output_statement = 108,         /* output_statement  */
  YYSYMBOL_print_list = 109,               /* print_list  */
  YYSYMBOL_print_item_tail = 110,          /* print_item_tail  */
  YYSYMBOL_print_item = 111,               /* print_item  */
  YYSYMBOL_procedure_call = 112,           /* procedure_call  */
  YYSYMBOL_actual_parameters_opt = 113,    /* actual_parameters_opt  */
  YYSYMBOL_actual_parameters = 114,        /* actual_parameters  */
  YYSYMBOL_actual_parameter_tail = 115,    /* actual_parameter_tail  */
  YYSYMBOL_actual_parameter = 116,         /* actual_parameter  */
  YYSYMBOL_conditional_statement = 117,    /* conditional_statement  */
  YYSYMBOL_if_statement = 118,             /* if_statement  */
  YYSYMBOL_condition = 119,                /* condition  */
  YYSYMBOL_if_else_statement = 120,        /* if_else_statement  */
  YYSYMBOL_if_else_if_statement = 121,     /* if_else_if_statement  */
  YYSYMBOL_else_if_tail = 122,             /* else_if_tail  */
  YYSYMBOL_optional_else = 123,            /* optional_else  */
  YYSYMBOL_repetition_statement = 124,     /* repetition_statement  */
  YYSYMBOL_for_statement = 125,            /* for_statement  */
  YYSYMBOL_repetition_range = 126,         /* repetition_range  */
  YYSYMBOL_step_opt = 127,                 /* step_opt  */
  YYSYMBOL_while_statement = 128,          /* while_statement  */
  YYSYMBOL_repeat_until_statement = 129,   /* repeat_until_statement  */
  YYSYMBOL_expression = 130,               /* expression  */
  YYSYMBOL_relational_operator = 131,      /* relational_operator  */
  YYSYMBOL_simple_expression = 132,        /* simple_expression  */
  YYSYMBOL_sign_opt = 133,                 /* sign_opt  */
  YYSYMBOL_sign_operator = 134,            /* sign_operator  */
  YYSYMBOL_addition_tail = 135,            /* addition_tail  */
  YYSYMBOL_addition_operator = 136,        /* addition_operator  */
  YYSYMBOL_term = 137,                     /* term  */
  YYSYMBOL_multiplication_tail = 138,      /* multiplication_tail  */
  YYSYMBOL_multiplication_operator = 139,  /* multiplication_operator  */
  YYSYMBOL_factor = 140,                   /* factor  */
  YYSYMBOL_identifier_factor = 141,        /* identifier_factor  */
  YYSYMBOL_variable_access = 142,          /* variable_access  */
  YYSYMBOL_selector = 143,                 /* selector  */
  YYSYMBOL_indexed_selector = 144,         /* indexed_selector  */
  YYSYMBOL_field_selector = 145,           /* field_selector  */
  YYSYMBOL_constant_value = 146,           /* constant_value  */
  YYSYMBOL_numeric = 147,                  /* numeric  */
  YYSYMBOL_literal = 148,                  /* literal  */
  YYSYMBOL_boolean_value = 149,            /* boolean_value  */
  YYSYMBOL_program_name = 150,             /* program_name  */
  YYSYMBOL_constant_name = 151,            /* constant_name  */
  YYSYMBOL_type_name = 152,                /* type_name  */
  YYSYMBOL_field_name = 153,               /* field_name  */
  YYSYMBOL_variable_name = 154,            /* variable_name  */
  YYSYMBOL_procedure_name = 155            /* procedure_name  */
};
typedef enum yysymbol_kind_t yysymbol_kind_t;




#ifdef short
# undef short
#endif

/* On compilers that do not define __PTRDIFF_MAX__ etc., make sure
   <limits.h> and (if available) <stdint.h> are included
   so that the code can choose integer types of a good width.  */

#ifndef __PTRDIFF_MAX__
# include <limits.h> /* INFRINGES ON USER NAME SPACE */
# if defined __STDC_VERSION__ && 199901 <= __STDC_VERSION__
#  include <stdint.h> /* INFRINGES ON USER NAME SPACE */
#  define YY_STDINT_H
# endif
#endif

/* Narrow types that promote to a signed type and that can represent a
   signed or unsigned integer of at least N bits.  In tables they can
   save space and decrease cache pressure.  Promoting to a signed type
   helps avoid bugs in integer arithmetic.  */

#ifdef __INT_LEAST8_MAX__
typedef __INT_LEAST8_TYPE__ yytype_int8;
#elif defined YY_STDINT_H
typedef int_least8_t yytype_int8;
#else
typedef signed char yytype_int8;
#endif

#ifdef __INT_LEAST16_MAX__
typedef __INT_LEAST16_TYPE__ yytype_int16;
#elif defined YY_STDINT_H
typedef int_least16_t yytype_int16;
#else
typedef short yytype_int16;
#endif

/* Work around bug in HP-UX 11.23, which defines these macros
   incorrectly for preprocessor constants.  This workaround can likely
   be removed in 2023, as HPE has promised support for HP-UX 11.23
   (aka HP-UX 11i v2) only through the end of 2022; see Table 2 of
   <https://h20195.www2.hpe.com/V2/getpdf.aspx/4AA4-7673ENW.pdf>.  */
#ifdef __hpux
# undef UINT_LEAST8_MAX
# undef UINT_LEAST16_MAX
# define UINT_LEAST8_MAX 255
# define UINT_LEAST16_MAX 65535
#endif

#if defined __UINT_LEAST8_MAX__ && __UINT_LEAST8_MAX__ <= __INT_MAX__
typedef __UINT_LEAST8_TYPE__ yytype_uint8;
#elif (!defined __UINT_LEAST8_MAX__ && defined YY_STDINT_H \
       && UINT_LEAST8_MAX <= INT_MAX)
typedef uint_least8_t yytype_uint8;
#elif !defined __UINT_LEAST8_MAX__ && UCHAR_MAX <= INT_MAX
typedef unsigned char yytype_uint8;
#else
typedef short yytype_uint8;
#endif

#if defined __UINT_LEAST16_MAX__ && __UINT_LEAST16_MAX__ <= __INT_MAX__
typedef __UINT_LEAST16_TYPE__ yytype_uint16;
#elif (!defined __UINT_LEAST16_MAX__ && defined YY_STDINT_H \
       && UINT_LEAST16_MAX <= INT_MAX)
typedef uint_least16_t yytype_uint16;
#elif !defined __UINT_LEAST16_MAX__ && USHRT_MAX <= INT_MAX
typedef unsigned short yytype_uint16;
#else
typedef int yytype_uint16;
#endif

#ifndef YYPTRDIFF_T
# if defined __PTRDIFF_TYPE__ && defined __PTRDIFF_MAX__
#  define YYPTRDIFF_T __PTRDIFF_TYPE__
#  define YYPTRDIFF_MAXIMUM __PTRDIFF_MAX__
# elif defined PTRDIFF_MAX
#  ifndef ptrdiff_t
#   include <stddef.h> /* INFRINGES ON USER NAME SPACE */
#  endif
#  define YYPTRDIFF_T ptrdiff_t
#  define YYPTRDIFF_MAXIMUM PTRDIFF_MAX
# else
#  define YYPTRDIFF_T long
#  define YYPTRDIFF_MAXIMUM LONG_MAX
# endif
#endif

#ifndef YYSIZE_T
# ifdef __SIZE_TYPE__
#  define YYSIZE_T __SIZE_TYPE__
# elif defined size_t
#  define YYSIZE_T size_t
# elif defined __STDC_VERSION__ && 199901 <= __STDC_VERSION__
#  include <stddef.h> /* INFRINGES ON USER NAME SPACE */
#  define YYSIZE_T size_t
# else
#  define YYSIZE_T unsigned
# endif
#endif

#define YYSIZE_MAXIMUM                                  \
  YY_CAST (YYPTRDIFF_T,                                 \
           (YYPTRDIFF_MAXIMUM < YY_CAST (YYSIZE_T, -1)  \
            ? YYPTRDIFF_MAXIMUM                         \
            : YY_CAST (YYSIZE_T, -1)))

#define YYSIZEOF(X) YY_CAST (YYPTRDIFF_T, sizeof (X))


/* Stored state numbers (used for stacks). */
typedef yytype_uint8 yy_state_t;

/* State numbers in computations.  */
typedef int yy_state_fast_t;

#ifndef YY_
# if defined YYENABLE_NLS && YYENABLE_NLS
#  if ENABLE_NLS
#   include <libintl.h> /* INFRINGES ON USER NAME SPACE */
#   define YY_(Msgid) dgettext ("bison-runtime", Msgid)
#  endif
# endif
# ifndef YY_
#  define YY_(Msgid) Msgid
# endif
#endif


#ifndef YY_ATTRIBUTE_PURE
# if defined __GNUC__ && 2 < __GNUC__ + (96 <= __GNUC_MINOR__)
#  define YY_ATTRIBUTE_PURE __attribute__ ((__pure__))
# else
#  define YY_ATTRIBUTE_PURE
# endif
#endif

#ifndef YY_ATTRIBUTE_UNUSED
# if defined __GNUC__ && 2 < __GNUC__ + (7 <= __GNUC_MINOR__)
#  define YY_ATTRIBUTE_UNUSED __attribute__ ((__unused__))
# else
#  define YY_ATTRIBUTE_UNUSED
# endif
#endif

/* Suppress unused-variable warnings by "using" E.  */
#if ! defined lint || defined __GNUC__
# define YY_USE(E) ((void) (E))
#else
# define YY_USE(E) /* empty */
#endif

/* Suppress an incorrect diagnostic about yylval being uninitialized.  */
#if defined __GNUC__ && ! defined __ICC && 406 <= __GNUC__ * 100 + __GNUC_MINOR__
# if __GNUC__ * 100 + __GNUC_MINOR__ < 407
#  define YY_IGNORE_MAYBE_UNINITIALIZED_BEGIN                           \
    _Pragma ("GCC diagnostic push")                                     \
    _Pragma ("GCC diagnostic ignored \"-Wuninitialized\"")
# else
#  define YY_IGNORE_MAYBE_UNINITIALIZED_BEGIN                           \
    _Pragma ("GCC diagnostic push")                                     \
    _Pragma ("GCC diagnostic ignored \"-Wuninitialized\"")              \
    _Pragma ("GCC diagnostic ignored \"-Wmaybe-uninitialized\"")
# endif
# define YY_IGNORE_MAYBE_UNINITIALIZED_END      \
    _Pragma ("GCC diagnostic pop")
#else
# define YY_INITIAL_VALUE(Value) Value
#endif
#ifndef YY_IGNORE_MAYBE_UNINITIALIZED_BEGIN
# define YY_IGNORE_MAYBE_UNINITIALIZED_BEGIN
# define YY_IGNORE_MAYBE_UNINITIALIZED_END
#endif
#ifndef YY_INITIAL_VALUE
# define YY_INITIAL_VALUE(Value) /* Nothing. */
#endif

#if defined __cplusplus && defined __GNUC__ && ! defined __ICC && 6 <= __GNUC__
# define YY_IGNORE_USELESS_CAST_BEGIN                          \
    _Pragma ("GCC diagnostic push")                            \
    _Pragma ("GCC diagnostic ignored \"-Wuseless-cast\"")
# define YY_IGNORE_USELESS_CAST_END            \
    _Pragma ("GCC diagnostic pop")
#endif
#ifndef YY_IGNORE_USELESS_CAST_BEGIN
# define YY_IGNORE_USELESS_CAST_BEGIN
# define YY_IGNORE_USELESS_CAST_END
#endif


#define YY_ASSERT(E) ((void) (0 && (E)))

#if 1

/* The parser invokes alloca or malloc; define the necessary symbols.  */

# ifdef YYSTACK_USE_ALLOCA
#  if YYSTACK_USE_ALLOCA
#   ifdef __GNUC__
#    define YYSTACK_ALLOC __builtin_alloca
#   elif defined __BUILTIN_VA_ARG_INCR
#    include <alloca.h> /* INFRINGES ON USER NAME SPACE */
#   elif defined _AIX
#    define YYSTACK_ALLOC __alloca
#   elif defined _MSC_VER
#    include <malloc.h> /* INFRINGES ON USER NAME SPACE */
#    define alloca _alloca
#   else
#    define YYSTACK_ALLOC alloca
#    if ! defined _ALLOCA_H && ! defined EXIT_SUCCESS
#     include <stdlib.h> /* INFRINGES ON USER NAME SPACE */
      /* Use EXIT_SUCCESS as a witness for stdlib.h.  */
#     ifndef EXIT_SUCCESS
#      define EXIT_SUCCESS 0
#     endif
#    endif
#   endif
#  endif
# endif

# ifdef YYSTACK_ALLOC
   /* Pacify GCC's 'empty if-body' warning.  */
#  define YYSTACK_FREE(Ptr) do { /* empty */; } while (0)
#  ifndef YYSTACK_ALLOC_MAXIMUM
    /* The OS might guarantee only one guard page at the bottom of the stack,
       and a page size can be as small as 4096 bytes.  So we cannot safely
       invoke alloca (N) if N exceeds 4096.  Use a slightly smaller number
       to allow for a few compiler-allocated temporary stack slots.  */
#   define YYSTACK_ALLOC_MAXIMUM 4032 /* reasonable circa 2006 */
#  endif
# else
#  define YYSTACK_ALLOC YYMALLOC
#  define YYSTACK_FREE YYFREE
#  ifndef YYSTACK_ALLOC_MAXIMUM
#   define YYSTACK_ALLOC_MAXIMUM YYSIZE_MAXIMUM
#  endif
#  if (defined __cplusplus && ! defined EXIT_SUCCESS \
       && ! ((defined YYMALLOC || defined malloc) \
             && (defined YYFREE || defined free)))
#   include <stdlib.h> /* INFRINGES ON USER NAME SPACE */
#   ifndef EXIT_SUCCESS
#    define EXIT_SUCCESS 0
#   endif
#  endif
#  ifndef YYMALLOC
#   define YYMALLOC malloc
#   if ! defined malloc && ! defined EXIT_SUCCESS
void *malloc (YYSIZE_T); /* INFRINGES ON USER NAME SPACE */
#   endif
#  endif
#  ifndef YYFREE
#   define YYFREE free
#   if ! defined free && ! defined EXIT_SUCCESS
void free (void *); /* INFRINGES ON USER NAME SPACE */
#   endif
#  endif
# endif
#endif /* 1 */

#if (! defined yyoverflow \
     && (! defined __cplusplus \
         || (defined YYSTYPE_IS_TRIVIAL && YYSTYPE_IS_TRIVIAL)))

/* A type that is properly aligned for any stack member.  */
union yyalloc
{
  yy_state_t yyss_alloc;
  YYSTYPE yyvs_alloc;
};

/* The size of the maximum gap between one aligned stack and the next.  */
# define YYSTACK_GAP_MAXIMUM (YYSIZEOF (union yyalloc) - 1)

/* The size of an array large to enough to hold all stacks, each with
   N elements.  */
# define YYSTACK_BYTES(N) \
     ((N) * (YYSIZEOF (yy_state_t) + YYSIZEOF (YYSTYPE)) \
      + YYSTACK_GAP_MAXIMUM)

# define YYCOPY_NEEDED 1

/* Relocate STACK from its old location to the new one.  The
   local variables YYSIZE and YYSTACKSIZE give the old and new number of
   elements in the stack, and YYPTR gives the new location of the
   stack.  Advance YYPTR to a properly aligned location for the next
   stack.  */
# define YYSTACK_RELOCATE(Stack_alloc, Stack)                           \
    do                                                                  \
      {                                                                 \
        YYPTRDIFF_T yynewbytes;                                         \
        YYCOPY (&yyptr->Stack_alloc, Stack, yysize);                    \
        Stack = &yyptr->Stack_alloc;                                    \
        yynewbytes = yystacksize * YYSIZEOF (*Stack) + YYSTACK_GAP_MAXIMUM; \
        yyptr += yynewbytes / YYSIZEOF (*yyptr);                        \
      }                                                                 \
    while (0)

#endif

#if defined YYCOPY_NEEDED && YYCOPY_NEEDED
/* Copy COUNT objects from SRC to DST.  The source and destination do
   not overlap.  */
# ifndef YYCOPY
#  if defined __GNUC__ && 1 < __GNUC__
#   define YYCOPY(Dst, Src, Count) \
      __builtin_memcpy (Dst, Src, YY_CAST (YYSIZE_T, (Count)) * sizeof (*(Src)))
#  else
#   define YYCOPY(Dst, Src, Count)              \
      do                                        \
        {                                       \
          YYPTRDIFF_T yyi;                      \
          for (yyi = 0; yyi < (Count); yyi++)   \
            (Dst)[yyi] = (Src)[yyi];            \
        }                                       \
      while (0)
#  endif
# endif
#endif /* !YYCOPY_NEEDED */

/* YYFINAL -- State number of the termination state.  */
#define YYFINAL  5
/* YYLAST -- Last index in YYTABLE.  */
#define YYLAST   216

/* YYNTOKENS -- Number of terminals.  */
#define YYNTOKENS  66
/* YYNNTS -- Number of nonterminals.  */
#define YYNNTS  90
/* YYNRULES -- Number of rules.  */
#define YYNRULES  163
/* YYNSTATES -- Number of states.  */
#define YYNSTATES  256

/* YYMAXUTOK -- Last valid token kind.  */
#define YYMAXUTOK   320


/* YYTRANSLATE(TOKEN-NUM) -- Symbol number corresponding to TOKEN-NUM
   as returned by yylex, with out-of-bounds checking.  */
#define YYTRANSLATE(YYX)                                \
  (0 <= (YYX) && (YYX) <= YYMAXUTOK                     \
   ? YY_CAST (yysymbol_kind_t, yytranslate[YYX])        \
   : YYSYMBOL_YYUNDEF)

/* YYTRANSLATE[TOKEN-NUM] -- Symbol number corresponding to TOKEN-NUM
   as returned by yylex.  */
static const yytype_int8 yytranslate[] =
{
       0,     2,     2,     2,     2,     2,     2,     2,     2,     2,
       2,     2,     2,     2,     2,     2,     2,     2,     2,     2,
       2,     2,     2,     2,     2,     2,     2,     2,     2,     2,
       2,     2,     2,     2,     2,     2,     2,     2,     2,     2,
       2,     2,     2,     2,     2,     2,     2,     2,     2,     2,
       2,     2,     2,     2,     2,     2,     2,     2,     2,     2,
       2,     2,     2,     2,     2,     2,     2,     2,     2,     2,
       2,     2,     2,     2,     2,     2,     2,     2,     2,     2,
       2,     2,     2,     2,     2,     2,     2,     2,     2,     2,
       2,     2,     2,     2,     2,     2,     2,     2,     2,     2,
       2,     2,     2,     2,     2,     2,     2,     2,     2,     2,
       2,     2,     2,     2,     2,     2,     2,     2,     2,     2,
       2,     2,     2,     2,     2,     2,     2,     2,     2,     2,
       2,     2,     2,     2,     2,     2,     2,     2,     2,     2,
       2,     2,     2,     2,     2,     2,     2,     2,     2,     2,
       2,     2,     2,     2,     2,     2,     2,     2,     2,     2,
       2,     2,     2,     2,     2,     2,     2,     2,     2,     2,
       2,     2,     2,     2,     2,     2,     2,     2,     2,     2,
       2,     2,     2,     2,     2,     2,     2,     2,     2,     2,
       2,     2,     2,     2,     2,     2,     2,     2,     2,     2,
       2,     2,     2,     2,     2,     2,     2,     2,     2,     2,
       2,     2,     2,     2,     2,     2,     2,     2,     2,     2,
       2,     2,     2,     2,     2,     2,     2,     2,     2,     2,
       2,     2,     2,     2,     2,     2,     2,     2,     2,     2,
       2,     2,     2,     2,     2,     2,     2,     2,     2,     2,
       2,     2,     2,     2,     2,     2,     1,     2,     3,     4,
       5,     6,     7,     8,     9,    10,    11,    12,    13,    14,
      15,    16,    17,    18,    19,    20,    21,    22,    23,    24,
      25,    26,    27,    28,    29,    30,    31,    32,    33,    34,
      35,    36,    37,    38,    39,    40,    41,    42,    43,    44,
      45,    46,    47,    48,    49,    50,    51,    52,    53,    54,
      55,    56,    57,    58,    59,    60,    61,    62,    63,    64,
      65
};

#if YYDEBUG
/* YYRLINE[YYN] -- Source line where rule number YYN was defined.  */
static const yytype_int16 yyrline[] =
{
       0,   577,   577,   586,   591,   592,   602,   607,   611,   614,
     623,   628,   629,   634,   643,   644,   649,   654,   655,   660,
     665,   666,   678,   687,   692,   697,   698,   703,   708,   709,
     718,   719,   724,   729,   730,   735,   740,   745,   746,   755,
     756,   761,   766,   767,   772,   777,   787,   792,   793,   798,
     803,   804,   809,   814,   815,   816,   825,   826,   827,   828,
     829,   830,   839,   844,   845,   850,   851,   852,   853,   854,
     855,   856,   857,   872,   881,   890,   895,   900,   901,   906,
     907,   916,   921,   922,   927,   932,   933,   945,   963,   964,
     965,   974,   979,   988,   997,  1011,  1012,  1024,  1025,  1034,
    1035,  1036,  1045,  1050,  1060,  1061,  1070,  1079,  1088,  1089,
    1094,  1095,  1096,  1097,  1098,  1099,  1104,  1109,  1110,  1115,
    1116,  1121,  1122,  1127,  1128,  1133,  1138,  1139,  1144,  1145,
    1146,  1147,  1148,  1149,  1158,  1159,  1160,  1161,  1162,  1163,
    1179,  1180,  1189,  1190,  1195,  1196,  1201,  1206,  1215,  1216,
    1217,  1218,  1223,  1224,  1229,  1230,  1235,  1236,  1245,  1250,
    1255,  1260,  1265,  1270
};
#endif

/** Accessing symbol of state STATE.  */
#define YY_ACCESSING_SYMBOL(State) YY_CAST (yysymbol_kind_t, yystos[State])

#if 1
/* The user-facing name of the symbol whose (internal) number is
   YYSYMBOL.  No bounds checking.  */
static const char *yysymbol_name (yysymbol_kind_t yysymbol) YY_ATTRIBUTE_UNUSED;

/* YYTNAME[SYMBOL-NUM] -- String name of the symbol SYMBOL-NUM.
   First, the terminals, then, starting at YYNTOKENS, nonterminals.  */
static const char *const yytname[] =
{
  "\"end of file\"", "error", "\"invalid token\"", "PROGRAM", "CONST",
  "TYPE", "LIST", "OF", "RECORD", "VAR", "PROCEDURE", "BY_VALUE",
  "BY_REFERENCE", "INT_TYPE", "REAL_TYPE", "BOOL_TYPE", "CHAR_TYPE",
  "STRING_TYPE", "READ", "PRINT", "IF", "THEN", "ELSE", "FOR", "TO",
  "STEP", "WHILE", "DO", "REPEAT", "UNTIL", "TRUE", "FALSE", "LT", "GT",
  "LE", "GE", "EQ", "NE", "PLUS", "MINUS", "OR", "POWER", "MUL",
  "REAL_DIV", "INT_DIV", "MOD", "AND", "NOT", "ASSIGN", "DOT", "COLON",
  "SEMICOLON", "SEMI_ELSE", "COMMA", "LBRACE", "RBRACE", "LBRACKET",
  "RBRACKET", "LPAREN", "RPAREN", "IDENTIFIER", "INTEGER_LITERAL",
  "REAL_LITERAL", "STRING_LITERAL", "CHAR_LITERAL", "LEXICAL_ERROR",
  "$accept", "program", "program_block", "declaration_part_opt",
  "declaration_part", "constant_definitions", "constant_definition_tail",
  "constant_definition", "type_definitions_opt", "type_definitions",
  "type_definition_tail", "type_definition", "compound_type", "list_type",
  "record_type", "field_list", "field_definition_tail", "field_definition",
  "identifier_tail", "variable_definitions_opt", "variable_definitions",
  "variable_definition_tail", "variable_definition", "variable_group",
  "variable_name_tail", "procedure_definitions_opt",
  "procedure_definitions", "procedure_definition_tail",
  "procedure_definition", "procedure_header", "procedure_block",
  "formal_parameters_opt", "formal_parameters", "formal_parameter_tail",
  "formal_parameter", "pass_mode_opt", "data_type", "instruction_list",
  "instruction_tail", "instruction", "assignment_statement",
  "input_statement", "output_statement", "print_list", "print_item_tail",
  "print_item", "procedure_call", "actual_parameters_opt",
  "actual_parameters", "actual_parameter_tail", "actual_parameter",
  "conditional_statement", "if_statement", "condition",
  "if_else_statement", "if_else_if_statement", "else_if_tail",
  "optional_else", "repetition_statement", "for_statement",
  "repetition_range", "step_opt", "while_statement",
  "repeat_until_statement", "expression", "relational_operator",
  "simple_expression", "sign_opt", "sign_operator", "addition_tail",
  "addition_operator", "term", "multiplication_tail",
  "multiplication_operator", "factor", "identifier_factor",
  "variable_access", "selector", "indexed_selector", "field_selector",
  "constant_value", "numeric", "literal", "boolean_value", "program_name",
  "constant_name", "type_name", "field_name", "variable_name",
  "procedure_name", YY_NULLPTR
};

static const char *
yysymbol_name (yysymbol_kind_t yysymbol)
{
  return yytname[yysymbol];
}
#endif

#define YYPACT_NINF (-178)

#define yypact_value_is_default(Yyn) \
  ((Yyn) == YYPACT_NINF)

#define YYTABLE_NINF (-164)

#define yytable_value_is_error(Yyn) \
  0

/* YYPACT[STATE-NUM] -- Index in YYTABLE of the portion describing
   STATE-NUM.  */
static const yytype_int16 yypact[] =
{
      29,   -20,    67,  -178,    24,  -178,    80,    22,    39,    50,
      58,    70,    66,  -178,   116,   113,   114,  -178,  -178,    80,
    -178,  -178,    77,  -178,  -178,    78,  -178,  -178,    72,  -178,
    -178,    73,  -178,     5,  -178,   113,  -178,   114,  -178,  -178,
    -178,   114,  -178,    76,    22,    17,    39,    94,    50,  -178,
      23,     0,    74,    79,    81,    82,    83,     5,    -5,  -178,
    -178,  -178,  -178,  -178,  -178,  -178,    -9,  -178,  -178,  -178,
    -178,  -178,  -178,    85,    86,   114,  -178,  -178,  -178,  -178,
    -178,  -178,  -178,  -178,  -178,  -178,    84,  -178,  -178,  -178,
    -178,  -178,    87,    91,    95,  -178,  -178,  -178,     4,    50,
    -178,  -178,    88,  -178,  -178,    50,    68,    -6,    11,    50,
      11,   101,    89,    11,  -178,  -178,  -178,   -25,     5,   118,
      11,    -3,  -178,  -178,    92,    89,  -178,  -178,  -178,  -178,
    -178,  -178,  -178,  -178,  -178,    97,    99,  -178,   -15,    93,
      96,  -178,  -178,  -178,  -178,  -178,    98,  -178,    61,     8,
    -178,   100,   106,   102,   107,  -178,  -178,   109,     5,  -178,
    -178,  -178,  -178,   105,  -178,  -178,  -178,   110,   103,  -178,
    -178,  -178,    75,  -178,  -178,   115,   135,  -178,  -178,  -178,
    -178,  -178,  -178,    11,     8,    11,   -15,  -178,  -178,  -178,
    -178,  -178,  -178,     5,    11,   142,    11,  -178,  -178,    -7,
    -178,   117,   127,  -178,   120,    38,  -178,    -6,     5,  -178,
    -178,   119,  -178,    69,    71,  -178,   148,     5,   121,     5,
     118,  -178,    11,     4,    89,     4,   122,  -178,  -178,  -178,
    -178,  -178,     8,  -178,  -178,  -178,  -178,  -178,  -178,     8,
      11,  -178,  -178,  -178,  -178,  -178,  -178,  -178,  -178,  -178,
    -178,  -178,   149,    11,  -178,  -178
};

/* YYDEFACT[STATE-NUM] -- Default reduction number in state STATE-NUM.
   Performed when YYTABLE does not specify something else to do.  Zero
   means the default is an error.  */
static const yytype_uint8 yydefact[] =
{
       0,     0,     0,   158,     0,     1,     4,     0,     0,     0,
       0,     0,     0,     5,    14,    30,    39,     9,    42,     4,
     159,    11,     0,   160,    17,     0,   162,    33,     0,    37,
     163,     0,     2,    72,     3,    30,    15,    39,    31,     8,
      40,    41,    46,     0,    10,     0,    16,     0,    32,    35,
       0,    47,     0,     0,     0,     0,     0,    72,   142,    71,
      63,    65,    66,    67,    68,    69,    88,    89,    90,    70,
      99,   100,   101,     0,     0,    39,     7,    43,    44,    12,
     156,   157,   152,   153,   154,   155,     0,   148,   149,   150,
     151,    18,     0,     0,     0,    20,    21,    34,     0,     0,
      54,    55,     0,    48,    50,     0,     0,     0,   117,     0,
     117,     0,     0,   117,   143,   144,   145,     0,    72,     0,
     117,   117,     6,    13,     0,     0,    19,    56,    57,    58,
      59,    60,    36,    61,    38,     0,    49,    52,   142,     0,
       0,    77,    79,    80,   119,   120,     0,    92,   108,     0,
     118,     0,     0,     0,     0,   161,   147,     0,    72,    62,
      93,    95,    73,     0,    83,    85,    87,     0,     0,    25,
      28,    45,    53,    74,    75,    76,     0,   110,   111,   112,
     113,   114,   115,   117,     0,   117,   140,   121,   126,   137,
     134,   135,   136,    72,   117,     0,   117,   146,    64,    97,
      81,    84,     0,    23,    24,     0,    51,     0,    72,   109,
     139,     0,   141,   116,   125,   102,     0,    72,     0,    72,
       0,    94,   117,     0,     0,     0,     0,    78,    91,   138,
     124,   123,     0,   128,   129,   130,   131,   132,   133,     0,
     117,   106,   107,    98,    96,    86,    22,    26,    27,    29,
     122,   127,   104,   117,   103,   105
};

/* YYPGOTO[NTERM-NUM].  */
static const yytype_int16 yypgoto[] =
{
    -178,  -178,   154,  -178,  -178,  -178,  -178,   131,  -178,   162,
    -178,   133,  -178,  -178,  -178,  -178,  -178,   -47,  -178,   146,
       7,  -178,   136,    90,  -178,   -29,    10,  -178,   144,  -178,
    -178,  -178,  -178,  -178,    14,  -178,  -120,   171,  -178,   -57,
    -178,  -178,  -178,  -178,  -178,   -19,  -178,  -178,  -178,  -178,
     -35,  -178,  -114,  -104,  -178,  -178,  -178,  -178,  -178,  -178,
    -178,  -178,  -178,  -178,  -111,  -178,     9,  -178,   -24,  -178,
    -178,   -42,  -178,  -178,  -177,  -178,  -103,    12,  -178,  -178,
    -178,   151,   -44,   152,  -178,   155,     6,   104,   -72,   181
};

/* YYDEFGOTO[NTERM-NUM].  */
static const yytype_uint8 yydefgoto[] =
{
       0,     2,    11,    12,    13,    14,    44,    21,    35,    15,
      46,    24,    94,    95,    96,   168,   204,   169,   205,    37,
      16,    48,    27,    28,    50,    39,    40,    41,    18,    19,
      43,   102,   103,   136,   104,   105,   132,    59,   117,    60,
      61,    62,    63,   140,   175,   141,    64,   163,   164,   201,
     165,    65,    66,   146,    67,    68,   199,   221,    69,    70,
     151,   254,    71,    72,   147,   183,   148,   149,   150,   213,
     232,   187,   214,   239,   188,   189,    73,   114,   115,   116,
      86,   190,   191,   192,     4,    22,   133,   170,    29,    74
};

/* YYTABLE[YYPACT[STATE-NUM]] -- What to do in state STATE-NUM.  If
   positive, shift that token.  If negative, reduce the rule whose
   number is the opposite.  If YYTABLE_NINF, syntax error.  */
static const yytype_int16 yytable[] =
{
     111,    88,   157,   139,   142,   161,   153,   210,    76,   162,
     166,   100,   101,   118,    25,   219,    17,   127,   128,   129,
     130,   131,    38,    52,    53,    54,   158,   134,    55,    17,
     159,    56,     1,    57,   112,   144,   145,   152,    80,    81,
       3,   113,    38,   119,   112,   220,   122,    80,    81,   144,
     145,   113,    25,  -163,   138,   184,   -82,    84,    85,    33,
     -53,   160,   251,   143,    23,    58,   185,     5,   186,    82,
      83,    84,    85,    98,   211,     6,    99,    20,    82,    83,
      84,    85,    20,   216,     7,     8,   100,   101,   225,     9,
      10,   226,   218,   177,   178,   179,   180,   181,   182,    23,
      92,   198,    93,   246,   142,   248,   244,   144,   145,   230,
      26,   166,   233,   234,   235,   236,   237,   238,    30,    32,
      33,     8,     9,    49,    10,    45,    47,    78,   138,   252,
     154,    51,   106,   120,   223,   123,   215,   107,    54,   108,
     109,   110,   255,   124,   121,   125,   126,   135,   171,   155,
     172,   228,   173,   167,   194,   174,   208,   176,   203,   193,
     241,   195,   243,   143,   200,   196,   197,   202,   207,   217,
     222,   224,   240,    42,   253,    79,    36,   247,   229,    91,
     242,    75,   249,    34,    97,    77,   206,   245,   227,   231,
     250,    31,   209,     0,     0,   137,    87,    89,   212,     0,
      90,     0,     0,     0,     0,     0,     0,     0,     0,     0,
       0,     0,     0,     0,     0,     0,   156
};

static const yytype_int16 yycheck[] =
{
      57,    45,   113,   106,   107,   119,   110,   184,    37,   120,
     121,    11,    12,    22,     8,    22,     6,    13,    14,    15,
      16,    17,    15,    18,    19,    20,    51,    99,    23,    19,
      55,    26,     3,    28,    49,    38,    39,   109,    30,    31,
      60,    56,    35,    52,    49,    52,    75,    30,    31,    38,
      39,    56,    46,    58,    60,    47,    59,    63,    64,    54,
      60,   118,   239,   107,    60,    60,    58,     0,    60,    61,
      62,    63,    64,    50,   185,    51,    53,    60,    61,    62,
      63,    64,    60,   194,     4,     5,    11,    12,    50,     9,
      10,    53,   196,    32,    33,    34,    35,    36,    37,    60,
       6,   158,     8,   223,   207,   225,   220,    38,    39,    40,
      60,   222,    41,    42,    43,    44,    45,    46,    60,    49,
      54,     5,     9,    51,    10,    48,    48,    51,    60,   240,
      29,    58,    58,    48,     7,    51,   193,    58,    20,    58,
      58,    58,   253,    56,    58,    54,    51,    59,    51,    60,
      51,   208,    59,    61,    48,    59,    21,    59,    55,    59,
     217,    59,   219,   207,    59,    58,    57,    57,    53,    27,
      53,    51,    24,    19,    25,    44,    14,   224,    59,    46,
      59,    35,    60,    12,    48,    41,   172,   222,   207,   213,
     232,    10,   183,    -1,    -1,   105,    45,    45,   186,    -1,
      45,    -1,    -1,    -1,    -1,    -1,    -1,    -1,    -1,    -1,
      -1,    -1,    -1,    -1,    -1,    -1,   112
};

/* YYSTOS[STATE-NUM] -- The symbol kind of the accessing symbol of
   state STATE-NUM.  */
static const yytype_uint8 yystos[] =
{
       0,     3,    67,    60,   150,     0,    51,     4,     5,     9,
      10,    68,    69,    70,    71,    75,    86,    92,    94,    95,
      60,    73,   151,    60,    77,   152,    60,    88,    89,   154,
      60,   155,    49,    54,   103,    74,    75,    85,    86,    91,
      92,    93,    68,    96,    72,    48,    76,    48,    87,    51,
      90,    58,    18,    19,    20,    23,    26,    28,    60,   103,
     105,   106,   107,   108,   112,   117,   118,   120,   121,   124,
     125,   128,   129,   142,   155,    85,    91,    94,    51,    73,
      30,    31,    61,    62,    63,    64,   146,   147,   148,   149,
     151,    77,     6,     8,    78,    79,    80,    88,    50,    53,
      11,    12,    97,    98,   100,   101,    58,    58,    58,    58,
      58,   105,    49,    56,   143,   144,   145,   104,    22,    52,
      48,    58,    91,    51,    56,    54,    51,    13,    14,    15,
      16,    17,   102,   152,   154,    59,    99,    89,    60,   142,
     109,   111,   142,   148,    38,    39,   119,   130,   132,   133,
     134,   126,   154,   119,    29,    60,   153,   130,    51,    55,
     105,   118,   130,   113,   114,   116,   130,    61,    81,    83,
     153,    51,    51,    59,    59,   110,    59,    32,    33,    34,
      35,    36,    37,   131,    47,    58,    60,   137,   140,   141,
     147,   148,   149,    59,    48,    59,    58,    57,   105,   122,
      59,   115,    57,    55,    82,    84,   100,    53,    21,   132,
     140,   130,   143,   135,   138,   105,   130,    27,   119,    22,
      52,   123,    53,     7,    51,    50,    53,   111,   105,    59,
      40,   134,   136,    41,    42,    43,    44,    45,    46,   139,
      24,   105,    59,   105,   118,   116,   102,    83,   102,    60,
     137,   140,   130,    25,   127,   130
};

/* YYR1[RULE-NUM] -- Symbol kind of the left-hand side of rule RULE-NUM.  */
static const yytype_uint8 yyr1[] =
{
       0,    66,    67,    68,    69,    69,    70,    70,    70,    70,
      71,    72,    72,    73,    74,    74,    75,    76,    76,    77,
      78,    78,    79,    80,    81,    82,    82,    83,    84,    84,
      85,    85,    86,    87,    87,    88,    89,    90,    90,    91,
      91,    92,    93,    93,    94,    95,    96,    97,    97,    98,
      99,    99,   100,   101,   101,   101,   102,   102,   102,   102,
     102,   102,   103,   104,   104,   105,   105,   105,   105,   105,
     105,   105,   105,   106,   107,   108,   109,   110,   110,   111,
     111,   112,   113,   113,   114,   115,   115,   116,   117,   117,
     117,   118,   119,   120,   121,   122,   122,   123,   123,   124,
     124,   124,   125,   126,   127,   127,   128,   129,   130,   130,
     131,   131,   131,   131,   131,   131,   132,   133,   133,   134,
     134,   135,   135,   136,   136,   137,   138,   138,   139,   139,
     139,   139,   139,   139,   140,   140,   140,   140,   140,   140,
     141,   141,   142,   142,   143,   143,   144,   145,   146,   146,
     146,   146,   147,   147,   148,   148,   149,   149,   150,   151,
     152,   153,   154,   155
};

/* YYR2[RULE-NUM] -- Number of symbols on the right-hand side of rule RULE-NUM.  */
static const yytype_int8 yyr2[] =
{
       0,     2,     5,     2,     0,     1,     4,     3,     2,     1,
       3,     0,     2,     4,     0,     1,     3,     0,     2,     4,
       1,     1,     6,     4,     2,     0,     3,     4,     0,     3,
       0,     1,     3,     0,     2,     2,     4,     0,     3,     0,
       1,     2,     0,     2,     3,     6,     1,     0,     1,     2,
       0,     3,     2,     0,     1,     1,     1,     1,     1,     1,
       1,     1,     4,     0,     3,     1,     1,     1,     1,     1,
       1,     1,     0,     3,     4,     4,     2,     0,     3,     1,
       1,     4,     0,     1,     2,     0,     3,     1,     1,     1,
       1,     6,     1,     3,     5,     0,     3,     0,     2,     1,
       1,     1,     5,     6,     0,     2,     6,     6,     1,     3,
       1,     1,     1,     1,     1,     1,     3,     0,     1,     1,
       1,     0,     3,     1,     1,     2,     0,     3,     1,     1,
       1,     1,     1,     1,     1,     1,     1,     1,     3,     2,
       1,     2,     1,     2,     1,     1,     3,     2,     1,     1,
       1,     1,     1,     1,     1,     1,     1,     1,     1,     1,
       1,     1,     1,     1
};


enum { YYENOMEM = -2 };

#define yyerrok         (yyerrstatus = 0)
#define yyclearin       (yychar = YYEMPTY)

#define YYACCEPT        goto yyacceptlab
#define YYABORT         goto yyabortlab
#define YYERROR         goto yyerrorlab
#define YYNOMEM         goto yyexhaustedlab


#define YYRECOVERING()  (!!yyerrstatus)

#define YYBACKUP(Token, Value)                                    \
  do                                                              \
    if (yychar == YYEMPTY)                                        \
      {                                                           \
        yychar = (Token);                                         \
        yylval = (Value);                                         \
        YYPOPSTACK (yylen);                                       \
        yystate = *yyssp;                                         \
        goto yybackup;                                            \
      }                                                           \
    else                                                          \
      {                                                           \
        yyerror (YY_("syntax error: cannot back up")); \
        YYERROR;                                                  \
      }                                                           \
  while (0)

/* Backward compatibility with an undocumented macro.
   Use YYerror or YYUNDEF. */
#define YYERRCODE YYUNDEF


/* Enable debugging if requested.  */
#if YYDEBUG

# ifndef YYFPRINTF
#  include <stdio.h> /* INFRINGES ON USER NAME SPACE */
#  define YYFPRINTF fprintf
# endif

# define YYDPRINTF(Args)                        \
do {                                            \
  if (yydebug)                                  \
    YYFPRINTF Args;                             \
} while (0)




# define YY_SYMBOL_PRINT(Title, Kind, Value, Location)                    \
do {                                                                      \
  if (yydebug)                                                            \
    {                                                                     \
      YYFPRINTF (stderr, "%s ", Title);                                   \
      yy_symbol_print (stderr,                                            \
                  Kind, Value); \
      YYFPRINTF (stderr, "\n");                                           \
    }                                                                     \
} while (0)


/*-----------------------------------.
| Print this symbol's value on YYO.  |
`-----------------------------------*/

static void
yy_symbol_value_print (FILE *yyo,
                       yysymbol_kind_t yykind, YYSTYPE const * const yyvaluep)
{
  FILE *yyoutput = yyo;
  YY_USE (yyoutput);
  if (!yyvaluep)
    return;
  YY_IGNORE_MAYBE_UNINITIALIZED_BEGIN
  YY_USE (yykind);
  YY_IGNORE_MAYBE_UNINITIALIZED_END
}


/*---------------------------.
| Print this symbol on YYO.  |
`---------------------------*/

static void
yy_symbol_print (FILE *yyo,
                 yysymbol_kind_t yykind, YYSTYPE const * const yyvaluep)
{
  YYFPRINTF (yyo, "%s %s (",
             yykind < YYNTOKENS ? "token" : "nterm", yysymbol_name (yykind));

  yy_symbol_value_print (yyo, yykind, yyvaluep);
  YYFPRINTF (yyo, ")");
}

/*------------------------------------------------------------------.
| yy_stack_print -- Print the state stack from its BOTTOM up to its |
| TOP (included).                                                   |
`------------------------------------------------------------------*/

static void
yy_stack_print (yy_state_t *yybottom, yy_state_t *yytop)
{
  YYFPRINTF (stderr, "Stack now");
  for (; yybottom <= yytop; yybottom++)
    {
      int yybot = *yybottom;
      YYFPRINTF (stderr, " %d", yybot);
    }
  YYFPRINTF (stderr, "\n");
}

# define YY_STACK_PRINT(Bottom, Top)                            \
do {                                                            \
  if (yydebug)                                                  \
    yy_stack_print ((Bottom), (Top));                           \
} while (0)


/*------------------------------------------------.
| Report that the YYRULE is going to be reduced.  |
`------------------------------------------------*/

static void
yy_reduce_print (yy_state_t *yyssp, YYSTYPE *yyvsp,
                 int yyrule)
{
  int yylno = yyrline[yyrule];
  int yynrhs = yyr2[yyrule];
  int yyi;
  YYFPRINTF (stderr, "Reducing stack by rule %d (line %d):\n",
             yyrule - 1, yylno);
  /* The symbols being reduced.  */
  for (yyi = 0; yyi < yynrhs; yyi++)
    {
      YYFPRINTF (stderr, "   $%d = ", yyi + 1);
      yy_symbol_print (stderr,
                       YY_ACCESSING_SYMBOL (+yyssp[yyi + 1 - yynrhs]),
                       &yyvsp[(yyi + 1) - (yynrhs)]);
      YYFPRINTF (stderr, "\n");
    }
}

# define YY_REDUCE_PRINT(Rule)          \
do {                                    \
  if (yydebug)                          \
    yy_reduce_print (yyssp, yyvsp, Rule); \
} while (0)

/* Nonzero means print parse trace.  It is left uninitialized so that
   multiple parsers can coexist.  */
int yydebug;
#else /* !YYDEBUG */
# define YYDPRINTF(Args) ((void) 0)
# define YY_SYMBOL_PRINT(Title, Kind, Value, Location)
# define YY_STACK_PRINT(Bottom, Top)
# define YY_REDUCE_PRINT(Rule)
#endif /* !YYDEBUG */


/* YYINITDEPTH -- initial size of the parser's stacks.  */
#ifndef YYINITDEPTH
# define YYINITDEPTH 200
#endif

/* YYMAXDEPTH -- maximum size the stacks can grow to (effective only
   if the built-in stack extension method is used).

   Do not make this value too large; the results are undefined if
   YYSTACK_ALLOC_MAXIMUM < YYSTACK_BYTES (YYMAXDEPTH)
   evaluated with infinite-precision integer arithmetic.  */

#ifndef YYMAXDEPTH
# define YYMAXDEPTH 10000
#endif


/* Context of a parse error.  */
typedef struct
{
  yy_state_t *yyssp;
  yysymbol_kind_t yytoken;
} yypcontext_t;

/* Put in YYARG at most YYARGN of the expected tokens given the
   current YYCTX, and return the number of tokens stored in YYARG.  If
   YYARG is null, return the number of expected tokens (guaranteed to
   be less than YYNTOKENS).  Return YYENOMEM on memory exhaustion.
   Return 0 if there are more than YYARGN expected tokens, yet fill
   YYARG up to YYARGN. */
static int
yypcontext_expected_tokens (const yypcontext_t *yyctx,
                            yysymbol_kind_t yyarg[], int yyargn)
{
  /* Actual size of YYARG. */
  int yycount = 0;
  int yyn = yypact[+*yyctx->yyssp];
  if (!yypact_value_is_default (yyn))
    {
      /* Start YYX at -YYN if negative to avoid negative indexes in
         YYCHECK.  In other words, skip the first -YYN actions for
         this state because they are default actions.  */
      int yyxbegin = yyn < 0 ? -yyn : 0;
      /* Stay within bounds of both yycheck and yytname.  */
      int yychecklim = YYLAST - yyn + 1;
      int yyxend = yychecklim < YYNTOKENS ? yychecklim : YYNTOKENS;
      int yyx;
      for (yyx = yyxbegin; yyx < yyxend; ++yyx)
        if (yycheck[yyx + yyn] == yyx && yyx != YYSYMBOL_YYerror
            && !yytable_value_is_error (yytable[yyx + yyn]))
          {
            if (!yyarg)
              ++yycount;
            else if (yycount == yyargn)
              return 0;
            else
              yyarg[yycount++] = YY_CAST (yysymbol_kind_t, yyx);
          }
    }
  if (yyarg && yycount == 0 && 0 < yyargn)
    yyarg[0] = YYSYMBOL_YYEMPTY;
  return yycount;
}




#ifndef yystrlen
# if defined __GLIBC__ && defined _STRING_H
#  define yystrlen(S) (YY_CAST (YYPTRDIFF_T, strlen (S)))
# else
/* Return the length of YYSTR.  */
static YYPTRDIFF_T
yystrlen (const char *yystr)
{
  YYPTRDIFF_T yylen;
  for (yylen = 0; yystr[yylen]; yylen++)
    continue;
  return yylen;
}
# endif
#endif

#ifndef yystpcpy
# if defined __GLIBC__ && defined _STRING_H && defined _GNU_SOURCE
#  define yystpcpy stpcpy
# else
/* Copy YYSRC to YYDEST, returning the address of the terminating '\0' in
   YYDEST.  */
static char *
yystpcpy (char *yydest, const char *yysrc)
{
  char *yyd = yydest;
  const char *yys = yysrc;

  while ((*yyd++ = *yys++) != '\0')
    continue;

  return yyd - 1;
}
# endif
#endif

#ifndef yytnamerr
/* Copy to YYRES the contents of YYSTR after stripping away unnecessary
   quotes and backslashes, so that it's suitable for yyerror.  The
   heuristic is that double-quoting is unnecessary unless the string
   contains an apostrophe, a comma, or backslash (other than
   backslash-backslash).  YYSTR is taken from yytname.  If YYRES is
   null, do not copy; instead, return the length of what the result
   would have been.  */
static YYPTRDIFF_T
yytnamerr (char *yyres, const char *yystr)
{
  if (*yystr == '"')
    {
      YYPTRDIFF_T yyn = 0;
      char const *yyp = yystr;
      for (;;)
        switch (*++yyp)
          {
          case '\'':
          case ',':
            goto do_not_strip_quotes;

          case '\\':
            if (*++yyp != '\\')
              goto do_not_strip_quotes;
            else
              goto append;

          append:
          default:
            if (yyres)
              yyres[yyn] = *yyp;
            yyn++;
            break;

          case '"':
            if (yyres)
              yyres[yyn] = '\0';
            return yyn;
          }
    do_not_strip_quotes: ;
    }

  if (yyres)
    return yystpcpy (yyres, yystr) - yyres;
  else
    return yystrlen (yystr);
}
#endif


static int
yy_syntax_error_arguments (const yypcontext_t *yyctx,
                           yysymbol_kind_t yyarg[], int yyargn)
{
  /* Actual size of YYARG. */
  int yycount = 0;
  /* There are many possibilities here to consider:
     - If this state is a consistent state with a default action, then
       the only way this function was invoked is if the default action
       is an error action.  In that case, don't check for expected
       tokens because there are none.
     - The only way there can be no lookahead present (in yychar) is if
       this state is a consistent state with a default action.  Thus,
       detecting the absence of a lookahead is sufficient to determine
       that there is no unexpected or expected token to report.  In that
       case, just report a simple "syntax error".
     - Don't assume there isn't a lookahead just because this state is a
       consistent state with a default action.  There might have been a
       previous inconsistent state, consistent state with a non-default
       action, or user semantic action that manipulated yychar.
     - Of course, the expected token list depends on states to have
       correct lookahead information, and it depends on the parser not
       to perform extra reductions after fetching a lookahead from the
       scanner and before detecting a syntax error.  Thus, state merging
       (from LALR or IELR) and default reductions corrupt the expected
       token list.  However, the list is correct for canonical LR with
       one exception: it will still contain any token that will not be
       accepted due to an error action in a later state.
  */
  if (yyctx->yytoken != YYSYMBOL_YYEMPTY)
    {
      int yyn;
      if (yyarg)
        yyarg[yycount] = yyctx->yytoken;
      ++yycount;
      yyn = yypcontext_expected_tokens (yyctx,
                                        yyarg ? yyarg + 1 : yyarg, yyargn - 1);
      if (yyn == YYENOMEM)
        return YYENOMEM;
      else
        yycount += yyn;
    }
  return yycount;
}

/* Copy into *YYMSG, which is of size *YYMSG_ALLOC, an error message
   about the unexpected token YYTOKEN for the state stack whose top is
   YYSSP.

   Return 0 if *YYMSG was successfully written.  Return -1 if *YYMSG is
   not large enough to hold the message.  In that case, also set
   *YYMSG_ALLOC to the required number of bytes.  Return YYENOMEM if the
   required number of bytes is too large to store.  */
static int
yysyntax_error (YYPTRDIFF_T *yymsg_alloc, char **yymsg,
                const yypcontext_t *yyctx)
{
  enum { YYARGS_MAX = 5 };
  /* Internationalized format string. */
  const char *yyformat = YY_NULLPTR;
  /* Arguments of yyformat: reported tokens (one for the "unexpected",
     one per "expected"). */
  yysymbol_kind_t yyarg[YYARGS_MAX];
  /* Cumulated lengths of YYARG.  */
  YYPTRDIFF_T yysize = 0;

  /* Actual size of YYARG. */
  int yycount = yy_syntax_error_arguments (yyctx, yyarg, YYARGS_MAX);
  if (yycount == YYENOMEM)
    return YYENOMEM;

  switch (yycount)
    {
#define YYCASE_(N, S)                       \
      case N:                               \
        yyformat = S;                       \
        break
    default: /* Avoid compiler warnings. */
      YYCASE_(0, YY_("syntax error"));
      YYCASE_(1, YY_("syntax error, unexpected %s"));
      YYCASE_(2, YY_("syntax error, unexpected %s, expecting %s"));
      YYCASE_(3, YY_("syntax error, unexpected %s, expecting %s or %s"));
      YYCASE_(4, YY_("syntax error, unexpected %s, expecting %s or %s or %s"));
      YYCASE_(5, YY_("syntax error, unexpected %s, expecting %s or %s or %s or %s"));
#undef YYCASE_
    }

  /* Compute error message size.  Don't count the "%s"s, but reserve
     room for the terminator.  */
  yysize = yystrlen (yyformat) - 2 * yycount + 1;
  {
    int yyi;
    for (yyi = 0; yyi < yycount; ++yyi)
      {
        YYPTRDIFF_T yysize1
          = yysize + yytnamerr (YY_NULLPTR, yytname[yyarg[yyi]]);
        if (yysize <= yysize1 && yysize1 <= YYSTACK_ALLOC_MAXIMUM)
          yysize = yysize1;
        else
          return YYENOMEM;
      }
  }

  if (*yymsg_alloc < yysize)
    {
      *yymsg_alloc = 2 * yysize;
      if (! (yysize <= *yymsg_alloc
             && *yymsg_alloc <= YYSTACK_ALLOC_MAXIMUM))
        *yymsg_alloc = YYSTACK_ALLOC_MAXIMUM;
      return -1;
    }

  /* Avoid sprintf, as that infringes on the user's name space.
     Don't have undefined behavior even if the translation
     produced a string with the wrong number of "%s"s.  */
  {
    char *yyp = *yymsg;
    int yyi = 0;
    while ((*yyp = *yyformat) != '\0')
      if (*yyp == '%' && yyformat[1] == 's' && yyi < yycount)
        {
          yyp += yytnamerr (yyp, yytname[yyarg[yyi++]]);
          yyformat += 2;
        }
      else
        {
          ++yyp;
          ++yyformat;
        }
  }
  return 0;
}


/*-----------------------------------------------.
| Release the memory associated to this symbol.  |
`-----------------------------------------------*/

static void
yydestruct (const char *yymsg,
            yysymbol_kind_t yykind, YYSTYPE *yyvaluep)
{
  YY_USE (yyvaluep);
  if (!yymsg)
    yymsg = "Deleting";
  YY_SYMBOL_PRINT (yymsg, yykind, yyvaluep, yylocationp);

  YY_IGNORE_MAYBE_UNINITIALIZED_BEGIN
  YY_USE (yykind);
  YY_IGNORE_MAYBE_UNINITIALIZED_END
}


/* Lookahead token kind.  */
int yychar;

/* The semantic value of the lookahead symbol.  */
YYSTYPE yylval;
/* Number of syntax errors so far.  */
int yynerrs;




/*----------.
| yyparse.  |
`----------*/

int
yyparse (void)
{
    Compiler_ResetSyntaxError();
    yy_state_fast_t yystate = 0;
    /* Number of tokens to shift before error messages enabled.  */
    int yyerrstatus = 0;

    /* Refer to the stacks through separate pointers, to allow yyoverflow
       to reallocate them elsewhere.  */

    /* Their size.  */
    YYPTRDIFF_T yystacksize = YYINITDEPTH;

    /* The state stack: array, bottom, top.  */
    yy_state_t yyssa[YYINITDEPTH];
    yy_state_t *yyss = yyssa;
    yy_state_t *yyssp = yyss;

    /* The semantic value stack: array, bottom, top.  */
    YYSTYPE yyvsa[YYINITDEPTH];
    YYSTYPE *yyvs = yyvsa;
    YYSTYPE *yyvsp = yyvs;

  int yyn;
  /* The return value of yyparse.  */
  int yyresult;
  /* Lookahead symbol kind.  */
  yysymbol_kind_t yytoken = YYSYMBOL_YYEMPTY;
  /* The variables used to return semantic value and location from the
     action routines.  */
  YYSTYPE yyval;

  /* Buffer for error messages, and its allocated size.  */
  char yymsgbuf[128];
  char *yymsg = yymsgbuf;
  YYPTRDIFF_T yymsg_alloc = sizeof yymsgbuf;

#define YYPOPSTACK(N)   (yyvsp -= (N), yyssp -= (N))

  /* The number of symbols on the RHS of the reduced rule.
     Keep to zero when no symbol should be popped.  */
  int yylen = 0;

  YYDPRINTF ((stderr, "Starting parse\n"));

  yychar = YYEMPTY; /* Cause a token to be read.  */

  goto yysetstate;


/*------------------------------------------------------------.
| yynewstate -- push a new state, which is found in yystate.  |
`------------------------------------------------------------*/
yynewstate:
  /* In all cases, when you get here, the value and location stacks
     have just been pushed.  So pushing a state here evens the stacks.  */
  yyssp++;


/*--------------------------------------------------------------------.
| yysetstate -- set current state (the top of the stack) to yystate.  |
`--------------------------------------------------------------------*/
yysetstate:
  YYDPRINTF ((stderr, "Entering state %d\n", yystate));
  YY_ASSERT (0 <= yystate && yystate < YYNSTATES);
  YY_IGNORE_USELESS_CAST_BEGIN
  *yyssp = YY_CAST (yy_state_t, yystate);
  YY_IGNORE_USELESS_CAST_END
  YY_STACK_PRINT (yyss, yyssp);

  if (yyss + yystacksize - 1 <= yyssp)
#if !defined yyoverflow && !defined YYSTACK_RELOCATE
    YYNOMEM;
#else
    {
      /* Get the current used size of the three stacks, in elements.  */
      YYPTRDIFF_T yysize = yyssp - yyss + 1;

# if defined yyoverflow
      {
        /* Give user a chance to reallocate the stack.  Use copies of
           these so that the &'s don't force the real ones into
           memory.  */
        yy_state_t *yyss1 = yyss;
        YYSTYPE *yyvs1 = yyvs;

        /* Each stack pointer address is followed by the size of the
           data in use in that stack, in bytes.  This used to be a
           conditional around just the two extra args, but that might
           be undefined if yyoverflow is a macro.  */
        yyoverflow (YY_("memory exhausted"),
                    &yyss1, yysize * YYSIZEOF (*yyssp),
                    &yyvs1, yysize * YYSIZEOF (*yyvsp),
                    &yystacksize);
        yyss = yyss1;
        yyvs = yyvs1;
      }
# else /* defined YYSTACK_RELOCATE */
      /* Extend the stack our own way.  */
      if (YYMAXDEPTH <= yystacksize)
        YYNOMEM;
      yystacksize *= 2;
      if (YYMAXDEPTH < yystacksize)
        yystacksize = YYMAXDEPTH;

      {
        yy_state_t *yyss1 = yyss;
        union yyalloc *yyptr =
          YY_CAST (union yyalloc *,
                   YYSTACK_ALLOC (YY_CAST (YYSIZE_T, YYSTACK_BYTES (yystacksize))));
        if (! yyptr)
          YYNOMEM;
        YYSTACK_RELOCATE (yyss_alloc, yyss);
        YYSTACK_RELOCATE (yyvs_alloc, yyvs);
#  undef YYSTACK_RELOCATE
        if (yyss1 != yyssa)
          YYSTACK_FREE (yyss1);
      }
# endif

      yyssp = yyss + yysize - 1;
      yyvsp = yyvs + yysize - 1;

      YY_IGNORE_USELESS_CAST_BEGIN
      YYDPRINTF ((stderr, "Stack size increased to %ld\n",
                  YY_CAST (long, yystacksize)));
      YY_IGNORE_USELESS_CAST_END

      if (yyss + yystacksize - 1 <= yyssp)
        YYABORT;
    }
#endif /* !defined yyoverflow && !defined YYSTACK_RELOCATE */


  if (yystate == YYFINAL)
    YYACCEPT;

  goto yybackup;


/*-----------.
| yybackup.  |
`-----------*/
yybackup:
  /* Do appropriate processing given the current state.  Read a
     lookahead token if we need one and don't already have one.  */

  /* First try to decide what to do without reference to lookahead token.  */
  yyn = yypact[yystate];
  if (yypact_value_is_default (yyn))
    goto yydefault;

  /* Not known => get a lookahead token if don't already have one.  */

  /* YYCHAR is either empty, or end-of-input, or a valid lookahead.  */
  if (yychar == YYEMPTY)
    {
      YYDPRINTF ((stderr, "Reading a token\n"));
      yychar = yylex ();
    }

  if (yychar <= YYEOF)
    {
      yychar = YYEOF;
      yytoken = YYSYMBOL_YYEOF;
      YYDPRINTF ((stderr, "Now at end of input.\n"));
    }
  else if (yychar == YYerror)
    {
      /* The scanner already issued an error message, process directly
         to error recovery.  But do not keep the error token as
         lookahead, it is too special and may lead us to an endless
         loop in error recovery. */
      yychar = YYUNDEF;
      yytoken = YYSYMBOL_YYerror;
      goto yyerrlab1;
    }
  else
    {
      yytoken = YYTRANSLATE (yychar);
      YY_SYMBOL_PRINT ("Next token is", yytoken, &yylval, &yylloc);
    }

  /* If the proper action on seeing token YYTOKEN is to reduce or to
     detect an error, take that action.  */
  yyn += yytoken;
  if (yyn < 0 || YYLAST < yyn || yycheck[yyn] != yytoken)
    goto yydefault;
  yyn = yytable[yyn];
  if (yyn <= 0)
    {
      if (yytable_value_is_error (yyn))
        goto yyerrlab;
      yyn = -yyn;
      goto yyreduce;
    }

  /* Count tokens shifted since error; after three, turn off error
     status.  */
  if (yyerrstatus)
    yyerrstatus--;

  /* Shift the lookahead token.  */
  YY_SYMBOL_PRINT ("Shifting", yytoken, &yylval, &yylloc);
  yystate = yyn;
  YY_IGNORE_MAYBE_UNINITIALIZED_BEGIN
  *++yyvsp = yylval;
  YY_IGNORE_MAYBE_UNINITIALIZED_END

  /* Discard the shifted token.  */
  yychar = YYEMPTY;
  goto yynewstate;


/*-----------------------------------------------------------.
| yydefault -- do the default action for the current state.  |
`-----------------------------------------------------------*/
yydefault:
  yyn = yydefact[yystate];
  if (yyn == 0)
    goto yyerrlab;
  goto yyreduce;


/*-----------------------------.
| yyreduce -- do a reduction.  |
`-----------------------------*/
yyreduce:
  /* yyn is the number of a rule to reduce with.  */
  yylen = yyr2[yyn];

  /* If YYLEN is nonzero, implement the default value of the action:
     '$$ = $1'.

     Otherwise, the following line sets YYVAL to garbage.
     This behavior is undocumented and Bison
     users should not rely upon it.  Assigning to YYVAL
     unconditionally makes the parser a bit smaller, and it avoids a
     GCC warning that YYVAL may be used uninitialized.  */
  yyval = yyvsp[1-yylen];


  YY_REDUCE_PRINT (yyn);
  switch (yyn)
    {

#line 2120 "D:\\ArabicCompiler\\ArabicCompiler\\ArabicCompiler.Native\\Bison\\parser.tab.c"

      default: break;
    }
  /* User semantic actions sometimes alter yychar, and that requires
     that yytoken be updated with the new translation.  We take the
     approach of translating immediately before every use of yytoken.
     One alternative is translating here after every semantic action,
     but that translation would be missed if the semantic action invokes
     YYABORT, YYACCEPT, or YYERROR immediately after altering yychar or
     if it invokes YYBACKUP.  In the case of YYABORT or YYACCEPT, an
     incorrect destructor might then be invoked immediately.  In the
     case of YYERROR or YYBACKUP, subsequent parser actions might lead
     to an incorrect destructor call or verbose syntax error message
     before the lookahead is translated.  */
  YY_SYMBOL_PRINT ("-> $$ =", YY_CAST (yysymbol_kind_t, yyr1[yyn]), &yyval, &yyloc);

  YYPOPSTACK (yylen);
  yylen = 0;

  *++yyvsp = yyval;

  /* Now 'shift' the result of the reduction.  Determine what state
     that goes to, based on the state we popped back to and the rule
     number reduced by.  */
  {
    const int yylhs = yyr1[yyn] - YYNTOKENS;
    const int yyi = yypgoto[yylhs] + *yyssp;
    yystate = (0 <= yyi && yyi <= YYLAST && yycheck[yyi] == *yyssp
               ? yytable[yyi]
               : yydefgoto[yylhs]);
  }

  goto yynewstate;


/*--------------------------------------.
| yyerrlab -- here on detecting error.  |
`--------------------------------------*/
yyerrlab:
  /* Make sure we have latest lookahead translation.  See comments at
     user semantic actions for why this is necessary.  */
  yytoken = yychar == YYEMPTY ? YYSYMBOL_YYEMPTY : YYTRANSLATE (yychar);
  /* If not already recovering from an error, report this error.  */
  if (!yyerrstatus)
    {
      ++yynerrs;
      {
        yypcontext_t yyctx
          = {yyssp, yytoken};
        char const *yymsgp = YY_("syntax error");
        int yysyntax_error_status;
        yysyntax_error_status = yysyntax_error (&yymsg_alloc, &yymsg, &yyctx);
        if (yysyntax_error_status == 0)
          yymsgp = yymsg;
        else if (yysyntax_error_status == -1)
          {
            if (yymsg != yymsgbuf)
              YYSTACK_FREE (yymsg);
            yymsg = YY_CAST (char *,
                             YYSTACK_ALLOC (YY_CAST (YYSIZE_T, yymsg_alloc)));
            if (yymsg)
              {
                yysyntax_error_status
                  = yysyntax_error (&yymsg_alloc, &yymsg, &yyctx);
                yymsgp = yymsg;
              }
            else
              {
                yymsg = yymsgbuf;
                yymsg_alloc = sizeof yymsgbuf;
                yysyntax_error_status = YYENOMEM;
              }
          }
        yyerror (yymsgp);
        if (yysyntax_error_status == YYENOMEM)
          YYNOMEM;
      }
    }

  if (yyerrstatus == 3)
    {
      /* If just tried and failed to reuse lookahead token after an
         error, discard it.  */

      if (yychar <= YYEOF)
        {
          /* Return failure if at end of input.  */
          if (yychar == YYEOF)
            YYABORT;
        }
      else
        {
          yydestruct ("Error: discarding",
                      yytoken, &yylval);
          yychar = YYEMPTY;
        }
    }

  /* Else will try to reuse lookahead token after shifting the error
     token.  */
  goto yyerrlab1;


/*---------------------------------------------------.
| yyerrorlab -- error raised explicitly by YYERROR.  |
`---------------------------------------------------*/
yyerrorlab:
  /* Pacify compilers when the user code never invokes YYERROR and the
     label yyerrorlab therefore never appears in user code.  */
  if (0)
    YYERROR;
  ++yynerrs;

  /* Do not reclaim the symbols of the rule whose action triggered
     this YYERROR.  */
  YYPOPSTACK (yylen);
  yylen = 0;
  YY_STACK_PRINT (yyss, yyssp);
  yystate = *yyssp;
  goto yyerrlab1;


/*-------------------------------------------------------------.
| yyerrlab1 -- common code for both syntax error and YYERROR.  |
`-------------------------------------------------------------*/
yyerrlab1:
  yyerrstatus = 3;      /* Each real token shifted decrements this.  */

  /* Pop stack until we find a state that shifts the error token.  */
  for (;;)
    {
      yyn = yypact[yystate];
      if (!yypact_value_is_default (yyn))
        {
          yyn += YYSYMBOL_YYerror;
          if (0 <= yyn && yyn <= YYLAST && yycheck[yyn] == YYSYMBOL_YYerror)
            {
              yyn = yytable[yyn];
              if (0 < yyn)
                break;
            }
        }

      /* Pop the current state because it cannot handle the error token.  */
      if (yyssp == yyss)
        YYABORT;


      yydestruct ("Error: popping",
                  YY_ACCESSING_SYMBOL (yystate), yyvsp);
      YYPOPSTACK (1);
      yystate = *yyssp;
      YY_STACK_PRINT (yyss, yyssp);
    }

  YY_IGNORE_MAYBE_UNINITIALIZED_BEGIN
  *++yyvsp = yylval;
  YY_IGNORE_MAYBE_UNINITIALIZED_END


  /* Shift the error token.  */
  YY_SYMBOL_PRINT ("Shifting", YY_ACCESSING_SYMBOL (yyn), yyvsp, yylsp);

  yystate = yyn;
  goto yynewstate;


/*-------------------------------------.
| yyacceptlab -- YYACCEPT comes here.  |
`-------------------------------------*/
yyacceptlab:
  yyresult = 0;
  goto yyreturnlab;


/*-----------------------------------.
| yyabortlab -- YYABORT comes here.  |
`-----------------------------------*/
yyabortlab:
  yyresult = 1;
  goto yyreturnlab;


/*-----------------------------------------------------------.
| yyexhaustedlab -- YYNOMEM (memory exhaustion) comes here.  |
`-----------------------------------------------------------*/
yyexhaustedlab:
  yyerror (YY_("memory exhausted"));
  yyresult = 2;
  goto yyreturnlab;


/*----------------------------------------------------------.
| yyreturnlab -- parsing is finished, clean up and return.  |
`----------------------------------------------------------*/
yyreturnlab:
  if (yychar != YYEMPTY)
    {
      /* Make sure we have latest lookahead translation.  See comments at
         user semantic actions for why this is necessary.  */
      yytoken = YYTRANSLATE (yychar);
      yydestruct ("Cleanup: discarding lookahead",
                  yytoken, &yylval);
    }
  /* Do not reclaim the symbols of the rule whose action triggered
     this YYABORT or YYACCEPT.  */
  YYPOPSTACK (yylen);
  YY_STACK_PRINT (yyss, yyssp);
  while (yyssp != yyss)
    {
      yydestruct ("Cleanup: popping",
                  YY_ACCESSING_SYMBOL (+*yyssp), yyvsp);
      YYPOPSTACK (1);
    }
#ifndef yyoverflow
  if (yyss != yyssa)
    YYSTACK_FREE (yyss);
#endif
  if (yymsg != yymsgbuf)
    YYSTACK_FREE (yymsg);
  return yyresult;
}

#line 1274 "D:\\ArabicCompiler\\ArabicCompiler\\ArabicCompiler.Native\\Bison\\parser.y"



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
