// globalSetup runs in the Node worker BEFORE the test environment is created.
// This ensures TextEncoder/TextDecoder from Node's 'util' are on the global
// before react-router loads its own encoding modules.
import { TextEncoder, TextDecoder } from 'util';

export function setup() {
  Object.assign(global, { TextEncoder, TextDecoder });
}
