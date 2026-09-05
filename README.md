# Carsties — Microservices Udemy Course Repository (2026 version)

This is the repo for the remastered version of the Udemy course, released September 2026.

- Previous (2025) version: [carsties-2025](https://github.com/TryCatchLearn/carsties-2025)
- 2024 version: [carsties-2024](https://github.com/TryCatchLearn/carsties-2024)
- 2023 version: [carsties-2023](https://github.com/TryCatchLearn/Carsties)

You can see how this app was made by taking the [Udemy course](https://www.udemy.com/course/build-a-microservices-app-with-dotnet-and-nextjs-from-scratch/?couponCode=NEWCOURSEPROM).

## Running the app locally

1. Clone the repo onto your machine, in a user folder:

    ```
    git clone https://github.com/TryCatchLearn/carsties.git
    ```

2. Change into the project directory:

    ```
    cd carsties
    ```

3. Ensure Docker Desktop is installed. If not, download it and follow the install instructions for your OS [here](https://docs.docker.com/desktop/).

4. Build the services locally (this may take several minutes):

    ```
    docker compose build
    ```

5. Once the build completes, start the services:

    ```
    docker compose up -d
    ```

6. To run the app over HTTPS, install `mkcert` from [here](https://github.com/FiloSottile/mkcert). Then install the local Certificate Authority:

    ```
    mkcert -install
    ```

7. Generate a certificate and key to replace the ones used in the course. Change into the `devcerts` directory and run:

    ```
    cd devcerts
    mkcert -key-file carsties.local.key -cert-file carsties.local.crt app.carsties.test api.carsties.test id.carsties.test
    ```

8. Add an entry to your hosts file so you can reach the app by domain name (see [this guide](https://www.hostinger.com/tutorials/how-to-edit-hosts-file) if needed):

    ```
    127.0.0.1 id.carsties.test app.carsties.test api.carsties.test
    ```

9. You should now be able to browse to the app at https://app.carsties.test