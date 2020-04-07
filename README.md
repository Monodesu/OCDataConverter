# OCDataConverter
读取ini文件的内容并推送到mysql数据库中

依赖mysql-connector-net
其安装包或dll文件已包含在MYSQLCONNECTORINSTALLER文件夹内。

将以下内容替换为你的服务器信息：
RDBServer = "Your Host";
RDBPort = "Your Host Port";
RDBUser = "Your Username";
RDBPassword = "Your Password";
RDBDatabase = "Your Database";

SelectPath：
选择你想要转换的ini文件所在目录
之后会自动遍历所有文件，请确保没有杂项。

readTest:
测试是否正常读取，成功之后再进行正式转换。

conver:
正式推送ini文件内容到mysql。

用于其他用途时只需更改与
query = "INSERT INTO info (uid, qq, mainmode) VALUES (" + UID + ", " + QQ + ", " + MainMode + ")";
相关的内容即可。
