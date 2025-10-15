import type { Config } from "jest";

const config: Config = {
  preset: "ts-jest",
  collectCoverage: true,
  testEnvironment: "jsdom",
  collectCoverageFrom: ["**/src/**/*.{ts,tsx}"],
  coverageDirectory: "./coverage/",
  transformIgnorePatterns: [],
  moduleNameMapper: {
    "^axios$": "axios/dist/node/axios.cjs",
    "\\.(css|less|scss|sass)$": "identity-obj-proxy"
  },
  transform: {
    '^.+\\.(ts|tsx)?$': 'ts-jest',
    '^.+\\.(js|jsx)$': 'babel-jest'
  },
  testMatch: [
    "<rootDir>/src/**/__tests__/**/*.{js,jsx,ts,tsx}",
    "<rootDir>/src/**/*.{spec,test}.{js,jsx,ts,tsx}"
  ],
  setupFiles: ['<rootDir>/setup.jest.js'],
};

export default config;
