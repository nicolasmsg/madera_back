@echo off
robocopy /e .\ConceptionDevisWS\bin "C:\Users\yves\Documents\My Web Sites\madera_back\bin" *.*
robocopy .\ConceptionDevisWS "C:\Users\yves\Documents\My Web Sites\madera_back" Web.config
robocopy .\ConceptionDevisWS "C:\Users\yves\Documents\My Web Sites\madera_back" Global.asax
pause