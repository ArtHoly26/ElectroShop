![Снимок экрана 2024-09-13 223643](https://github.com/user-attachments/assets/1eba32d7-ffd9-457e-990e-2f9dfd066451)
Разработано окно аутентификации пользователя. Для входа необходимо пройти регистрацию новому пользователю нажав кнопку Registration.

![Снимок экрана 2024-09-13 223724](https://github.com/user-attachments/assets/ccc1687a-c3bf-4869-8570-66ff33570a87)
Разработано окно авторизации пользователя с валидацией данных. Для регистрации нового пользователя необходимо нажать кнопку Registration.

![Снимок экрана 2024-09-13 223839](https://github.com/user-attachments/assets/c886673a-cdcb-4700-81e1-5d6747b239f4)
Для входа необходимо ввести Login и Password пользователя.
![Снимок экрана 2024-09-13 223939](https://github.com/user-attachments/assets/e3e9fc8e-5748-4408-975e-a8420f9a1c65)

Если данные введены не верно. Система оповестит пользователя об ошибке.
![Снимок экрана 2024-09-13 223939](https://github.com/user-attachments/assets/89599fca-8c37-4c6d-9875-9349289480d1)

При входе пользователь попадает в следующее разработанное окно MainMenu. На данны момент разработки сделанны три вкладки. Account, Catalog, Selected. И кнопка Log out.
![Снимок экрана 2024-09-13 224000](https://github.com/user-attachments/assets/47acf35f-ca0c-4289-8810-4c446703d852)

Во вкладке Account находится разработанное окно WindowAcccount, в нем находятся все данные о пользователе и возможность их изменять.
![Снимок экрана 2024-09-13 224513](https://github.com/user-attachments/assets/e6af0372-f77b-4439-98c8-b0d1b5ce03a1)
Во вкладке Catalog разработанно окно WindowCatalog, в котором есть возможность выбрать категорию товара , ознакомиться с каждым товаром и добавить его в избранное.
![Снимок экрана 2024-09-13 224059](https://github.com/user-attachments/assets/e82471e2-b065-4827-86ec-ab883150cbdb)
![Снимок экрана 2024-09-13 224124](https://github.com/user-attachments/assets/f773f3ab-981d-40b0-bd7c-ebdc62585460)
![Снимок экрана 2024-09-13 224144](https://github.com/user-attachments/assets/1d1e1758-252b-4d1c-8240-df66f9decfeb)
![Снимок экрана 2024-09-13 224219](https://github.com/user-attachments/assets/394c4f97-3fcd-4592-8467-b5af644f565c)
![Снимок экрана 2024-09-13 224236](https://github.com/user-attachments/assets/d3f28cce-d0de-4d2e-a8c8-c212b70d599b)
![Снимок экрана 2024-09-13 224326](https://github.com/user-attachments/assets/7f211c15-5726-4ae4-af35-b83705042753)
Во вкладке Selected разработанно окно WindowSelected, в котором находится избранный товар из каталога. Разработанна возможность удалять товар из избранного.
![Снимок экрана 2024-09-13 224410](https://github.com/user-attachments/assets/5d6d6a9c-e49c-4449-ab27-c08df94a144b)
![Снимок экрана 2024-09-13 224441](https://github.com/user-attachments/assets/85839fdc-b91a-456d-9748-881afb97cff9)

При разработке использовался локальная база данных MSSQL и ADO.NET — это библиотека классов, при помощи которых можно устанавливать подключение к базе данных и выполнять запросы. В будущем планирется перенести все данные на удаленную БД.

Создание базы данных ElectroShop.
![Снимок экрана 2024-09-13 231109](https://github.com/user-attachments/assets/2a570905-e134-4c4d-ac4c-352705bd228f)

Таблица со всеми данными пользователя.
![Снимок экрана 2024-09-13 231125](https://github.com/user-attachments/assets/2390e395-0e4d-4f88-85fe-d39337ebb876)

Таблица со всеми категориями товартов.
![Снимок экрана 2024-09-13 231229](https://github.com/user-attachments/assets/9eaeaeb5-8f15-40a5-ae5d-8381a1251ff4)

Таблица со всеми продуктами.
![Снимок экрана 2024-09-13 231152](https://github.com/user-attachments/assets/09531fd4-a31f-4046-8505-caade0602aad)

Таблица со всеми избранными товарами.
![Снимок экрана 2024-09-13 231211](https://github.com/user-attachments/assets/4b1c9525-7f23-4420-8df2-d06ec86e9104)


В будущем:
- планируется разделение по ролям пользователей. Например: Администратор, Менеджер магазина, Клиент;
- сортировки по товарам;
- поиск;
- корзина покупок;
- отправка сообщения на почту;
- настройка приложения (внешний вид, язык и другой функционал).
- сравнение товаров.
