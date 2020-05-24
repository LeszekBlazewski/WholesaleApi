# Start docker containers

        docker-compose up -d

# Add this in DbContextClass

```
     protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseNpgsql("Host=localhost;Database=deliveries;Username=employee;Password=employee1");
```

# In visual studio choose as startup and default project in CLI - project Wholesale.DAL and then run this from package manager console

        Update-Database

# Now you can remove the line added in step 2

# Setup views/triggers/procedures by running everything through pgadmin

Docker compose runs also the pgadmin container which allows quick access to the running db.

1. Navigate to http://localhost:5050

login: pgadmin4@pgadmin.org

password: admin

2. Add new server with following properties:

Name: what_ever_you_want

Connection Tab

Host name: deliveries

Port 5432

Maintenance database: deliveries

Username: employee

Password: employee1

3. Execute the following queries in Query tool window

```SQL
CREATE FUNCTION public.check_order()
    RETURNS trigger
    LANGUAGE 'plpgsql'
AS $BODY$
DECLARE
    requestor_role users.role%TYPE;
    deliverer_role users.role%TYPE;
BEGIN
  SELECT role INTO requestor_role from users where user_id = NEW.client_id;
  SELECT role INTO deliverer_role from users where user_id = NEW.courier_id;
  IF requestor_role <> 'client'
  THEN
   RAISE EXCEPTION 'User is not a client ! Orders can be created only for clients !';
  ELSEIF deliverer_role IS NOT NULL AND deliverer_role <> 'courier'
  THEN
    RAISE EXCEPTION 'Only couriers can devliver packages !';
  END IF;

  IF NEW.date IS NULL
  THEN
    NEW.date = now();
  END IF;
  RETURN NEW;
END;
$BODY$;
```

```SQL
CREATE TRIGGER check_order_trigger
    BEFORE INSERT OR UPDATE
    ON public.orders
    FOR EACH ROW
    EXECUTE PROCEDURE public.check_order();
```

```SQL
CREATE OR REPLACE PROCEDURE public.orders_total_worth(
  start_date date,
  end_date date,
  INOUT total_worth numeric)
LANGUAGE 'sql'
AS $BODY$
SELECT SUM(od.amount * pr.price)
 FROM orders o
 JOIN order_details od USING (order_id)
 JOIN products pr USING (product_id)
 WHERE o.date > start_date AND o.date < end_date
$BODY$;
```

```SQL
CREATE VIEW public.courier_stats
 AS
 SELECT u.user_id AS courier_id,
    u.first_name,
    u.last_name,
    COUNT(DISTINCT o.order_id) AS number_of_orders,
    SUM(od.amount::numeric * pr.price) AS total_worth
   FROM users u
     LEFT JOIN orders o ON u.user_id = o.courier_id
     JOIN order_details od USING (order_id)
     JOIN products pr USING (product_id)
  WHERE o.status = 'completed'::order_status
  GROUP BY u.user_id, u.first_name, u.last_name
  ORDER BY u.last_name, u.first_name;
```

# using the app

To use the app you need to register with different roles client/courier but to access employee account you have to manually update the required Role filed in db for given account.

There are no accounts or any data predefined.
