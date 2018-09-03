CREATE TABLE IF NOT EXISTS users		(userId INTEGER PRIMARY KEY AUTOINCREMENT,
                                         userName TEXT NOT NULL UNIQUE,
										 userPass TEXT NOT NULL,
										 FirstName TEXT NOT NULL,
										 Surname TEXT NOT NULL,
										 DOB TEXT NOT NULL,
										 PhoneNumber TEXT NOT NULL,
										 HAddress TEXT NOT NULL,
										 EmailAddress TEXT NOT NULL);
CREATE TABLE IF NOT EXISTS roles        (roleId INTEGER PRIMARY KEY AUTOINCREMENT,
                                         roleName TEXT NOT NULL UNIQUE);
CREATE TABLE IF NOT EXISTS user_roles   (user_roles_id INTEGER PRIMARY KEY AUTOINCREMENT,
                                         user INTEGER NOT NULL,
										 role INTEGER NOT NULL,
										 FOREIGN KEY (user) REFERENCES users(userId),
										 FOREIGN KEY (role) REFERENCES roles(roleId));
INSERT OR IGNORE INTO roles VALUES (1, 'Developer');
INSERT OR IGNORE INTO roles VALUES (2, 'Admin');
INSERT OR IGNORE INTO roles VALUES (3, 'Student');
INSERT OR IGNORE INTO roles VALUES (4, 'Teacher');
CREATE VIEW IF NOT EXISTS UserRoleData AS
SELECT user_role_id AS UserRoleID, roleName, user
FROM user_roles
INNER JOIN roles ON user_roles.user = roles.roleId;

