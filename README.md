# MySQL-Dumper
A program that calls MySQL or MariaDB internally using the Process object in C#. It can be used to quickly create whole database dumps in .sql, and also restore them. Enjoy.

To use this program, simply click on the MySQLDumper_exe.
I created a 64 bit version as well, to efficiently work with large .sql dumps.
This is because the program has to comment out the following lines,
CREATE DATABASE
and 
USE <DATABASE>

After the dump has been created by the mysqldump.exe client.
We ussually comment out the lines so that on importing the file onto a server, it will use the new database name we give it.

Created by MNYIKKA
Email me at mnyikka@gmail.com
Enjoy!
