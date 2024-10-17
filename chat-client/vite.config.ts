import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'
import * as dotenv from 'dotenv';

dotenv.config();

// https://vitejs.dev/config/
export default defineConfig(({mode}) => {
  return {
    plugins: [vue()],
    server: {
      port: parseInt(process.env.VITE_APP_PORT ?? "3000", 10),
      open: true
    },
    define: {
      'pricess.env': {
        VITE_APP_MODE: mode
      }
    }
  }
})