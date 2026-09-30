/* A Bison parser, made by GNU Bison 3.8.2.  */

/* Bison interface for Yacc-like parsers in C

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

/* DO NOT RELY ON FEATURES THAT ARE NOT DOCUMENTED in the manual,
   especially those whose name start with YY_ or yy_.  They are
   private implementation details that can be changed or removed.  */

#ifndef YY_YY_D_ARABICCOMPILER_ARABICCOMPILER_ARABICCOMPILER_NATIVE_BISON_PARSER_TAB_H_INCLUDED
# define YY_YY_D_ARABICCOMPILER_ARABICCOMPILER_ARABICCOMPILER_NATIVE_BISON_PARSER_TAB_H_INCLUDED
/* Debug traces.  */
#ifndef YYDEBUG
# define YYDEBUG 0
#endif
#if YYDEBUG
extern int yydebug;
#endif

/* Token kinds.  */
#ifndef YYTOKENTYPE
# define YYTOKENTYPE
  enum yytokentype
  {
    YYEMPTY = -2,
    YYEOF = 0,                     /* "end of file"  */
    YYerror = 256,                 /* error  */
    YYUNDEF = 257,                 /* "invalid token"  */
    PROGRAM = 258,                 /* PROGRAM  */
    CONST = 259,                   /* CONST  */
    TYPE = 260,                    /* TYPE  */
    LIST = 261,                    /* LIST  */
    OF = 262,                      /* OF  */
    RECORD = 263,                  /* RECORD  */
    VAR = 264,                     /* VAR  */
    PROCEDURE = 265,               /* PROCEDURE  */
    BY_VALUE = 266,                /* BY_VALUE  */
    BY_REFERENCE = 267,            /* BY_REFERENCE  */
    INT_TYPE = 268,                /* INT_TYPE  */
    REAL_TYPE = 269,               /* REAL_TYPE  */
    BOOL_TYPE = 270,               /* BOOL_TYPE  */
    CHAR_TYPE = 271,               /* CHAR_TYPE  */
    STRING_TYPE = 272,             /* STRING_TYPE  */
    READ = 273,                    /* READ  */
    PRINT = 274,                   /* PRINT  */
    IF = 275,                      /* IF  */
    THEN = 276,                    /* THEN  */
    ELSE = 277,                    /* ELSE  */
    FOR = 278,                     /* FOR  */
    TO = 279,                      /* TO  */
    STEP = 280,                    /* STEP  */
    WHILE = 281,                   /* WHILE  */
    DO = 282,                      /* DO  */
    REPEAT = 283,                  /* REPEAT  */
    UNTIL = 284,                   /* UNTIL  */
    TRUE = 285,                    /* TRUE  */
    FALSE = 286,                   /* FALSE  */
    LT = 287,                      /* LT  */
    GT = 288,                      /* GT  */
    LE = 289,                      /* LE  */
    GE = 290,                      /* GE  */
    EQ = 291,                      /* EQ  */
    NE = 292,                      /* NE  */
    PLUS = 293,                    /* PLUS  */
    MINUS = 294,                   /* MINUS  */
    OR = 295,                      /* OR  */
    POWER = 296,                   /* POWER  */
    MUL = 297,                     /* MUL  */
    REAL_DIV = 298,                /* REAL_DIV  */
    INT_DIV = 299,                 /* INT_DIV  */
    MOD = 300,                     /* MOD  */
    AND = 301,                     /* AND  */
    NOT = 302,                     /* NOT  */
    ASSIGN = 303,                  /* ASSIGN  */
    DOT = 304,                     /* DOT  */
    COLON = 305,                   /* COLON  */
    SEMICOLON = 306,               /* SEMICOLON  */
    SEMI_ELSE = 307,               /* SEMI_ELSE  */
    COMMA = 308,                   /* COMMA  */
    LBRACE = 309,                  /* LBRACE  */
    RBRACE = 310,                  /* RBRACE  */
    LBRACKET = 311,                /* LBRACKET  */
    RBRACKET = 312,                /* RBRACKET  */
    LPAREN = 313,                  /* LPAREN  */
    RPAREN = 314,                  /* RPAREN  */
    IDENTIFIER = 315,              /* IDENTIFIER  */
    INTEGER_LITERAL = 316,         /* INTEGER_LITERAL  */
    REAL_LITERAL = 317,            /* REAL_LITERAL  */
    STRING_LITERAL = 318,          /* STRING_LITERAL  */
    CHAR_LITERAL = 319,            /* CHAR_LITERAL  */
    LEXICAL_ERROR = 320            /* LEXICAL_ERROR  */
  };
  typedef enum yytokentype yytoken_kind_t;
#endif

/* Value type.  */
#if ! defined YYSTYPE && ! defined YYSTYPE_IS_DECLARED
union YYSTYPE
{
#line 438 "D:\\ArabicCompiler\\ArabicCompiler\\ArabicCompiler.Native\\Bison\\parser.y"

    char* text;

#line 133 "D:\\ArabicCompiler\\ArabicCompiler\\ArabicCompiler.Native\\Bison\\parser.tab.h"

};
typedef union YYSTYPE YYSTYPE;
# define YYSTYPE_IS_TRIVIAL 1
# define YYSTYPE_IS_DECLARED 1
#endif


extern YYSTYPE yylval;


int yyparse (void);


#endif /* !YY_YY_D_ARABICCOMPILER_ARABICCOMPILER_ARABICCOMPILER_NATIVE_BISON_PARSER_TAB_H_INCLUDED  */
