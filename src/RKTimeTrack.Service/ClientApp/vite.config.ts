/**
 * Name: vite.config.ts
 * Description: Vite configuration file
 *
 * Original from https://khalidabuhakmeh.com/running-vite-with-aspnet-core-web-
 */

import {UserConfig, defineConfig, ProxyOptions} from 'vite';
import {fileURLToPath, URL} from 'node:url';
import vue from '@vitejs/plugin-vue';

import appsettingsDev from "../appsettings.Development.json";

export default defineConfig(({ command }) => {
  const isDevelopment = command !== "build";

  // Prepare proxy options
  const proxyOptions : Record<string, ProxyOptions> = {};
  if(isDevelopment){
    appsettingsDev.ViteDevelopmentServer.RoutesForAspNet.forEach(actRoute =>{
      proxyOptions[actRoute] = {
        target: appsettingsDev.Kestrel.Endpoints.Http.Url
      }
    })
  }
  console.log("## Proxy configuration for development server")
  console.log(proxyOptions);
  console.log("");

  // Vite configuration
  const config: UserConfig = {
    plugins: [
      vue(),
    ],
    resolve: {
      alias: {
        '@': fileURLToPath(new URL('./src', import.meta.url))
      }
    },
    build: {
      emptyOutDir: true,
      outDir: 'dist',
      sourcemap: isDevelopment
    },
    server: isDevelopment ? {
      proxy: proxyOptions,
      port: appsettingsDev.ViteDevelopmentServer.Port,
      strictPort: true,
      hmr: {
        host: "localhost",
        clientPort: appsettingsDev.ViteDevelopmentServer.Port
      }
    } : undefined
  }

  console.log("## Full vite configuration")
  console.log(config);
  console.log("");

  return config;
});